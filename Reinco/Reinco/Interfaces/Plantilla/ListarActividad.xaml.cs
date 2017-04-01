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
    public partial class ListarActividad : ContentPage, INotifyPropertyChanged
    {

        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;
        protected int IdPlantilla;


        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion



        #region ObservableCollections
        public ObservableCollection<ActividadItem> ActividadItems { get; set; } 
        #endregion



        public ListarActividad(int idPlantilla, string nombre)
        {
            InitializeComponent();
            this.Title = nombre;
            this.IdPlantilla = idPlantilla;

            ActividadItems = new ObservableCollection<ActividadItem>();
           
            CargarActividadItems();
            // eventos
            RefreshActividadCommand = new Command(() =>
            {
                ActividadItems.Clear();
                CargarActividadItems();
                IsRefreshingActividad = false;
            });

            AgregarActividad = new Command(() =>
            {
                Navigation.PushAsync(new AgregarActividad(IdPlantilla));
            });


            this.BindingContext = this;
        }




        #region=============Refrescar pagina=======================
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
            get
            {
                return isRefreshingActividad;
            }
        }
        #endregion



        #region +--- comandos ----+
        public ICommand RefreshActividadCommand { get; private set; }
        public ICommand AgregarActividad { get; private set; }
        #endregion



        #region +---- Definiendo Propiedad Global De esta Pagina ----+
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarActividad = this;
        }
        #endregion


        #region================= cargar actividades=====================
        public async void CargarActividadItems()
        {
            byte x = 01; // utilizada para la enumeracion de las actividades
            try
            {
                object[,] OidPlantilla = new object[,] { { "idPlantilla", IdPlantilla } };
                dynamic result = await Servicio.MetodoGet("ServicioPlantillaActividad.asmx", "MostrarActividadxIdPlantilla", OidPlantilla);
                foreach (var item in result)
                {
                    ActividadItems.Add(new ActividadItem
                    {
                        idActividad = item.idPlantilla_Actividad,
                        nombre = item.nombre,
                        tolerancia = item.tolerancia,
                        idPlantilla = item.idPlantilla,
                        enumera = x++,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion

        #region=============eliminar actividad=====================
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {
                var idActividad = ((MenuItem)sender).CommandParameter;
                int IdActividad = Convert.ToInt16(idActividad);
                bool respuesta = await DisplayAlert("Eliminar", "¿Desea eliminar este Actividad?", "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idPlantillaActividad", IdActividad } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantillaActividad.asmx", "EliminarPlantillaActividad", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Actividad", Mensaje, "OK");
                    App.ListarActividad.ActividadItems.Clear();
                    App.ListarActividad.CargarActividadItems();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Actividad", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }

        }
        #endregion



    }
}
