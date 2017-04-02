using Reinco.Entidades;
using Reinco.Recursos;
using System;
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
    public partial class ListarPlantilla : ContentPage, INotifyPropertyChanged
    {

        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;

        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region ObservableCollections
        public ObservableCollection<PlantillaItem> PlantillaItems { get; set; } 
        #endregion

        public ListarPlantilla()
        {
            InitializeComponent();

            PlantillaItems = new ObservableCollection<PlantillaItem>();

            // cargado las plantillas
            CargarPlantilla();

            // Comandos
            RefreshPlantillaCommand = new Command(() =>
            {
                PlantillaItems.Clear();
                CargarPlantilla();
                IsRefreshingPlantilla = false;
            });

            CrearPlantilla = new Command(() =>
            {
                Navigation.PushAsync(new AgregarPlantilla());
            });

            // Contecto de los bindings
            this.BindingContext = this;
        }


        #region +---- Refrescar Lista ----+
        private bool isRefreshingPlantilla { get; set; }
        public bool IsRefreshingPlantilla
        {
            set
            {
                if (isRefreshingPlantilla != value)
                {
                    isRefreshingPlantilla = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingPlantilla"));
                }
            }
            get
            {
                return isRefreshingPlantilla;
            }
        }
        #endregion

        #region +---- comandos ----+
        public ICommand RefreshPlantillaCommand { get; private set; }
        public ICommand CrearPlantilla { get; private set; }
        #endregion

        #region +---- Propiedad Global De esta Pagina ----+
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarPlantilla = this;
        }
        #endregion

        #region======================== cargar plantilla en lista====================================
        public async void CargarPlantilla()
        {
            try
            {
                dynamic plantillas = await Servicio.MetodoGet("ServicioPlantilla.asmx", "MostrarPlantillas");
                foreach (var plantilla in plantillas)
                {
                    PlantillaItems.Add(new PlantillaItem
                    {
                        idPlantilla = plantilla.idPlantilla,
                        codigo = plantilla.codigo,
                        nombre = plantilla.nombre,
                    });
                }

            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error", ex.Message);
            }
        }
        #endregion

        #region===================// Eliminar Plantilla CRUD //====================
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {
                // Recuperando el idPlantilla
                var idPlantilla = ((MenuItem)sender).CommandParameter;
                int IdPlantilla = Convert.ToInt16(idPlantilla);

                // Consumiendo datos de la web service
                bool respuesta = await DisplayAlert("Eliminar", "¿Desea eliminar esta plantilla? ", "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idPlantilla", IdPlantilla } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantilla.asmx", "EliminarPlantilla", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    // await mensaje.MostrarMensaje("Eliminar Plantilla", Mensaje);
                    await App.Current.MainPage.DisplayAlert("Eliminar Plantilla", Mensaje, "OK");
                    PlantillaItems.Clear();
                    CargarPlantilla();
                    //IsRefreshingPlantilla = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Plantilla", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        #endregion

    }
}
