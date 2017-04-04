using Newtonsoft.Json;
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

namespace Reinco.Interfaces.Propietario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarPropietario : ContentPage, INotifyPropertyChanged
    {
        WebService Servicio = new WebService();
        string Mensaje;
        public VentanaMensaje mensaje;


        private bool isRefreshingPropietario { get; set; }


        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion



        #region MyRegion
        public ObservableCollection<PropietarioItem> PropietarioItems { get; set; }
        public bool IsRefreshingPropietario
        {
            set
            {
                if (isRefreshingPropietario != value)
                {
                    isRefreshingPropietario = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingPropietario"));
                }
            }
            get
            {
                return isRefreshingPropietario;
            }
        }
        #endregion



        #region Comands
        public ICommand RefreshPropietarioCommand { get; private set; }
        public ICommand AgregarPropietario { get; private set; }
        #endregion




        // datatable usuario;
        #region=============constructor vacio======================
        public ListarPropietario()
        {
            InitializeComponent();
            PropietarioItems = new ObservableCollection<PropietarioItem>();
            CargarPropietarioItem();

            RefreshPropietarioCommand = new Command(() =>
            {
                PropietarioItems.Clear();
                CargarPropietarioItem();
                IsRefreshingPropietario = false;

            });

            AgregarPropietario = new Command(() =>
            {
                Navigation.PushAsync(new AgregarPropietario());
            });
            this.BindingContext = this;
        }
        #endregion

        #region +============= Definiendo Propiedad Global De esta Pagina ===========
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarPropietarios = this;
        }
        #endregion

        #region=============cargar propietarios==================
        public async void CargarPropietarioItem()
        {
            try
            {
                dynamic result = await Servicio.MetodoGet("ServicioPropietario.asmx", "MostrarPropietarios");
                foreach (var item in result)
                {
                    PropietarioItems.Add(new PropietarioItem
                    {
                        idPropietario = item.idPropietario,
                        nombre = item.nombre,
                        fotoPerfil = "ic_profile_color.png",
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
            
        }
        #endregion


        #region ===================// Eliminar Propietario====================
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {
                var idPropietario = ((MenuItem)sender).CommandParameter;
                int IdPropietario = Convert.ToInt16(idPropietario);
                bool respuesta = await DisplayAlert("Eliminar", "¿Desea eliminar al propietario?", "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idPropietario", IdPropietario} };
                dynamic result = await Servicio.MetodoGetString("ServicioPropietario.asmx", "EliminarPropietario", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Usuario", Mensaje, "OK");
                    await Navigation.PopAsync();

                    IsRefreshingPropietario = true;
                    PropietarioItems.Clear();
                    CargarPropietarioItem();
                    IsRefreshingPropietario = false;

                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Propietario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
           
        }

        #endregion

    }
}
