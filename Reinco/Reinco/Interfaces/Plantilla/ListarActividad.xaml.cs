using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarActividad : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;
        WebService Servicio = new WebService();
        PlantillaItem plantilla;

        public ObservableCollection<ActividadItem> ActividadItems { get; set; }
        private bool isRefreshingActividad { get; set; }
        public bool IsRefreshingActividad
        {
            set
            {
                if (isRefreshingActividad != value)
                {
                    isRefreshingActividad = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingActividad"));
                }
            }
            get { return isRefreshingActividad; }
        }
        public ICommand RefreshActividadCommand { get; private set; }
        public ICommand agregarActividad { get; set; }

        public ListarActividad(PlantillaItem Plantilla)
        {
            InitializeComponent();
            plantilla = Plantilla;

            directorio.Text = App.directorio + "/" + Plantilla.nombre + "/Actividades";

            this.Title = Plantilla.nombre;

            ActividadItems = new ObservableCollection<ActividadItem>();
            CargarActividad();

            // Commands
            RefreshActividadCommand = new Command(() =>
            {
                ActividadItems.Clear();
                CargarActividad();
            });
            agregarActividad = new Command(() =>
            {
                Navigation.PushAsync(new AgregarActividad(Plantilla));
            });

            // Contexto Para Los bindings
            this.BindingContext = this;
        }

        #region +---- Definiendo Propiedad Global De esta Pagina ----+
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarActividad = this;
        }
        #endregion

        public async void CargarActividad()
        {
            byte x = 01; // utilizada para la enumeracion de las actividades
            try
            {
                // IsRefreshingActividad = true;
                listaVacia.IsVisible = false;
                object[,] OidPlantilla = new object[,] { { "idPlantilla", plantilla.idPlantilla } };
                dynamic result = await Servicio.MetodoGet("ServicioPlantillaActividad.asmx", "MostrarActividadxIdPlantilla", OidPlantilla);
                if (result.Count == 0) //si está vacío
                {
                    listaVacia.IsVisible = true;
                    lblListaVacia.Text = "No hay Actividades para esta plantilla.";
                }
                foreach (var item in result)
                {
                    ActividadItems.Add(new ActividadItem
                    {
                        idActividad         = item.idPlantilla_Actividad,
                        nombre              = item.nombre,
                        tolerancia          = item.tolerancia,
                        idPlantilla         = item.idPlantilla,
                        enumera             = x++,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
                return;
            }
            finally
            {
                IsRefreshingActividad = false;
            }
        }

        #region ================================ Scroll Infinito ================================
        /*
            @ Evento que se dispara cadaves que el escroll lega al final de ventana
            ================================
                    SCROLL INFINITO
            ================================
        */
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = listViewPlantilla.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                // Aqui Logica de programacion cada ves que se ejecute este evento =====================================================//
                // int cargarNuevos = 5; // solo de prueva
                // int totalRegistroActual = PropietarioItems.Count(); // solo de prueva
                // CargarActividad();
            }
        }
        #endregion

    }
}
