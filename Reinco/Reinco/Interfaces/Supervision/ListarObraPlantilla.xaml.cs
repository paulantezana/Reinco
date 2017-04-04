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
    public partial class ListarObraPlantilla : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged; // Usada paralos refrescar las listas


        int IdObra;
        string NombreObra;
        int IdPropietarioObra;
        string Mensaje;

        public VentanaMensaje mensaje;
        private bool isRefreshingObraPlantilla { get; set; }
        public string DireccionApp { get; set; }

        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();





        public bool IsRefreshingObraPlantilla
        {
            set
            {
                if (isRefreshingObraPlantilla != value)
                {
                    isRefreshingObraPlantilla = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingObraPlantilla"));
                }
            }
            get
            {
                return isRefreshingObraPlantilla;
            }
        }

        public ObservableCollection<ObraPlantillaItem> ObraPlantillaItems { get; set; }

        public ICommand RefreshObraPlantillaCommand { get; private set; }
        public ICommand asignarPlantilla { get; private set; }

        public ListarObraPlantilla(int idPropietarioObra,int idObra, string nombreObra = "PLANTILLAS",string directorioApp = "")
        {
            InitializeComponent();
            IdObra = idObra;
            IdPropietarioObra = idPropietarioObra;
            NombreObra = nombreObra;

            this.Title = nombreObra;
            this.DireccionApp = directorioApp + "\\Plantillas";

            // ---------------------
            ObraPlantillaItems = new ObservableCollection<ObraPlantillaItem>();


            // Refrescar la lista
            RefreshObraPlantillaCommand = new Command(() =>
            {
                ObraPlantillaItems.Clear();
                CargarPlantillaObra();
                IsRefreshingObraPlantilla = false;
            });

            // comandos
            asignarPlantilla = new Command(() =>
            {
                Navigation.PushAsync(new AsignarPlantilla(IdPropietarioObra));
            });


            // Cargando la lista
            CargarPlantillaObra();

            // Contexto de los Bindings Clase Actual Importante para que pueda funcionar el refresco de la lista con Gestos
            this.BindingContext = this;  
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarObraPlantilla = this;
        }

        public async void CargarPlantillaObra()
        {

            try
            {
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idObra", IdObra } };
                dynamic result = await servicio.MetodoGet("ServicioPlantillaPropietarioObra.asmx", "MostrarPlantillaxidObra", variables);

                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        //await mensaje.MostrarMensaje("Mostrar Obra Plantilla", "No hay plantillas que mostrar");
                        await DisplayAlert("Información", "No hay plantillas", "Aceptar");
                        return;
                    }
                    else
                    {
                        // listando las obras
                        foreach (var item in result)
                        {
                            ObraPlantillaItems.Add(new ObraPlantillaItem
                            {
                                nombre = item.nombre,
                                codigo = item.codigo,
                                idPlantillaObra=item.idPlantilla_Propietario_obra,
                                idObra=item.idObra,
                                idPlantilla=item.idPlantilla
                            });
                        }
                        // fin del listado
                    }
                }
                else
                {
                    await mensaje.MostrarMensaje("Iniciar Sesión", "Error de respuesta del servicio, Contáctese con el administrador.");
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error:", ex.Message);
            }

        }
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {
                // Recuperando el idPlantilla
                var idPlantilla = ((MenuItem)sender).CommandParameter;
                int IdPlantillaPropietarioObra = Convert.ToInt16(idPlantilla);

                // Consumiendo datos de la web service
                bool respuesta = await DisplayAlert("Eliminar", "¿Desea eliminar esta plantilla de la obra? ", "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idPlantillaPropietarioObra", IdPlantillaPropietarioObra } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantillaPropietarioObra.asmx", "EliminarPlantillaPropietarioObra", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Plantilla Obra", Mensaje, "OK");
                    ObraPlantillaItems.Clear();
                    CargarPlantillaObra();
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Plantilla Obra", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }

    }
}
