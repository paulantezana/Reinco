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

        #region +---- Atributos ----+
        string IdObra;
        string NombreObra;

        public VentanaMensaje mensaje;
        private bool isRefreshingObraPlantilla { get; set; }
        #endregion

        #region +---- Services ----+
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        #endregion

        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion

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


        public ListarObraPlantilla(string idObra, string nombreObra = "PLANTILLAS")
        {
            InitializeComponent();
            IdObra = idObra;
            NombreObra = nombreObra;
            this.Title = nombreObra;

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
                Navigation.PushAsync(new AsignarPlantilla());
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



        private async void CargarPlantillaObra()
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
                        await mensaje.MostrarMensaje("Mostrar Obra Plantilla", "No hay plantillas que mostrar");
                    }
                    else
                    {
                        // listando las obras
                        foreach (var item in result)
                        {
                            ObraPlantillaItems.Add(new ObraPlantillaItem
                            {
                                nombre = "Gestion De Calidad",
                                codigo = "F",
                                descripcion = "Breve Descripción"
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


    }
}
