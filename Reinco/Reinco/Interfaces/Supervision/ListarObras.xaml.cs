using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarObras : ContentPage, INotifyPropertyChanged
    {
        #region +---- Atributos ----+
        public VentanaMensaje mensaje;
        int IdUsuario;
        //int IdObra;
        private bool isRefreshingObraResponsable { get; set; }
        //public ObservableCollection<ObraItem> ObraItems { get; set; }
        string Color;
        #endregion
        
        #region +---- Services ----+
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        #endregion

        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +---- Propiedades ----+
        public ObservableCollection<ObraResponsableItem> ObraResponsableItems { get; set; }
        public bool IsRefreshingObraResponsable
        {
            set
            {
                if (isRefreshingObraResponsable != value)
                {
                    isRefreshingObraResponsable = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingObraResponsable"));
                }
            }
            get
            {
                return isRefreshingObraResponsable;
            }
        }
        #endregion

        #region +---- Comandos ----+
        public ICommand RefreshObraCommand { get; private set; }
        #endregion

        #region Constructores
        public ListarObras(int idUsuario, string cargo = "")
        {
            InitializeComponent();
            mensaje = new VentanaMensaje();
            ObraResponsableItems = new ObservableCollection<ObraResponsableItem>();
            //ObraItems = new ObservableCollection<ObraItem>();
            IdUsuario = idUsuario;
            
            #region Cargar Obras Segun El Cargo
            if (cargo == "Gerente")
            {
                CargarObraAdminItems();
                this.Title = "Administrador";
            }
            if (cargo == "Responsable")
            {
                CargarObraResponsableItems();
                this.Title = "Responsable";
            }
            if (cargo == "Asistente")
            {
                this.Title = "Asistente";
                CargarObraSupervisorItems();
            } 
            #endregion

            RefreshObraCommand = new Command(() =>
            {
                ObraResponsableItems.Clear();
                // CargarObraResponsableItems();
                IsRefreshingObraResponsable = false;
            });

            this.BindingContext = this; // Contexto de los Bindings Clase Actual Importante para que pueda funcionar el refresco de la lista con Gestos
        }

        #endregion


        #region Global
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarObras = this;
        } 
        #endregion

        #region Cargar Obras Como Administrador
        public async void CargarObraAdminItems()
        {        
            try
            {
                // ObraResponsableItems.Clear();
                //servicioObra, mostrarObras--modificado
                
                dynamic obras = await Servicio.MetodoGet("ServicioPlantillaPropietarioObra.asmx", "MostrarPlantillasyObras");
                foreach (var item in obras)
                {
                    if (item.idPlantilla == null)
                        Color = "#FF7777";
                    else
                        Color = "#77FF77";
                    //IdObra = Convert.ToInt16( item.idObra);
                    ObraResponsableItems.Add(new ObraResponsableItem
                    {   
                        idPlantillaObra = item.idPlantilla == null ? 0 : item.idPlantilla,
                        idObra =item.idObra,
                        codigo = item.codigo,
                        nombre = item.nombre,
                        idPropietarioObra=item.idPropietario_obra,
                        colorObra = Color
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        #endregion

        #region Cargar Obras Como Responsable

        private async void CargarObraResponsableItems()
        {
            try
            {
                ObraResponsableItems.Clear();
                // Desde Aqui Logica de Programacion

                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idResponsable", IdUsuario } };
                dynamic result = await servicio.MetodoPost("ServicioObra.asmx", "MostrarObrasResponsable", variables);

                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        await mensaje.MostrarMensaje("Mostrar Obra Responsable", "No Hay Obras a su cargo");
                    }
                    else
                    {
                        // listando las obras
                        foreach (var item in result)
                        {
                            ObraResponsableItems.Add(new ObraResponsableItem
                            {
                                nombre = item.nombre,
                                idResponsable = item.idObra,
                            });
                        }
                        // fin del listado
                    }
                }
                else
                {
                    await mensaje.MostrarMensaje("Mostrar Obra Responsable", "A ocurrido un error al listar las obras para este usuario");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Cargar Obras Como Supervisor

        private async void CargarObraSupervisorItems()
        {
            try
            {
                // ObraResponsableItems.Clear();
                //servicioObra, mostrarObras--modificado
                object[,] variables = new object[,] { { "idUsuario", IdUsuario } };
                dynamic obras = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarObrasSupervision",variables);
                foreach (var item in obras)
                {
                    if (item.idPlantilla == null)
                        Color = "#FF7777";
                    else
                        Color = "#77FF77";
                    //IdObra = Convert.ToInt16( item.idObra);
                    ObraResponsableItems.Add(new ObraResponsableItem
                    {
                       // idPlantillaObra = item.idPlantilla == null ? 0 : item.idPlantilla,
                        idObra = item.idObra,
                        codigo = item.codigo,
                        nombre = item.nombre,
                        //idPropietarioObra = item.idPropietario_obra,
                        colorObra = Color
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion


    }
}
