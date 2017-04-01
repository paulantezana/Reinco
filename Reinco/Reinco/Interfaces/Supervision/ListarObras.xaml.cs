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
        private bool isRefreshingObraResponsable { get; set; }
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

        public ListarObras()
        {
            InitializeComponent();
            mensaje = new VentanaMensaje();
            ObraResponsableItems = new ObservableCollection<ObraResponsableItem>();
            CargarObrasAdmin();

            RefreshObraCommand = new Command(() =>
            {
                ObraResponsableItems.Clear();
                CargarObrasAdmin();
                IsRefreshingObraResponsable = false;
            });

            this.BindingContext = this; // Contexto de los Bindings Clase Actual Importante para que pueda funcionar el refresco de la lista con Gestos
        }

        //public ListarObras(object )
        //{
        //    InitializeComponent();
        //    mensaje = new VentanaMensaje();
        //    ObraResponsableItems = new ObservableCollection<ObraResponsableItem>();
        //    CargarObrasAdmin();

        //    RefreshObraCommand = new Command(() =>
        //    {
        //        ObraResponsableItems.Clear();
        //        CargarObrasAdmin();
        //        IsRefreshingObraResponsable = false;
        //    });

        //    this.BindingContext = this; // Contexto de los Bindings Clase Actual Importante para que pueda funcionar el refresco de la lista con Gestos
        //}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarObras = this;
        }
        #region===========Cargar todas las obras===================
        private async void CargarObrasAdmin()
        {
            try
            {

                // Recuperando el id Usuario
               // string recuperarIdUsuario = Application.Current.Properties["idUsuario"].ToString();
                //Int16 idUsuario = Convert.ToInt16(recuperarIdUsuario);

                // Iniciando Web Service
                WebService servicio = new WebService();
               // object[,] variables = new object[,] { { "idResponsable", idUsuario } };
                dynamic result = await servicio.MetodoGet("ServicioObra.asmx", "MostrarObrasResponsable");

                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        await mensaje.MostrarMensaje("Mostrar Obras", "No hay obras disponibles");
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
                    await mensaje.MostrarMensaje("Mostrar Obras", "A ocurrido un error al listar las obras.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
