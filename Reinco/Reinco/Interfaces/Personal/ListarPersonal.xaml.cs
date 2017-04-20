using Newtonsoft.Json;
using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections;
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

namespace Reinco.Interfaces.Personal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarPersonal : ContentPage, INotifyPropertyChanged
    {
        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;

        new public event PropertyChangedEventHandler PropertyChanged;


        #region Refrescar Lista
        private bool isRefreshingPersonal { get; set; }
        public bool IsRefreshingPersonal
        {
            set
            {
                if (isRefreshingPersonal != value)
                {
                    isRefreshingPersonal = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingPersonal"));
                }
            }
            get
            {
                return isRefreshingPersonal;
            }
        } 
        #endregion

        public ObservableCollection<PersonalItem> Personaltems { get; set; }

        public ICommand RefreshPersonalCommand { get; private set; }
        public ICommand AgregarPersonal { get; private set; }
        
        public ListarPersonal()
        {
            InitializeComponent();
            directorio.Text = App.directorio + "\\Personal";
            Personaltems = new ObservableCollection<PersonalItem>();
            CargarPersonalItem();

            RefreshPersonalCommand = new Command(() =>
            {
                Personaltems.Clear();
                CargarPersonalItem();
            });
            AgregarPersonal = new Command(() =>
            {
                Navigation.PushAsync(new AgregarPersonal());
            });

            // Contexto para los bindings
            this.BindingContext = this;
        }

        #region ====================== Propiedad Global De Esta Pagina ======================
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarPersonal = this;
        } 
        #endregion

        #region ============================== Cargar usuarios ==============================
        public async void CargarPersonalItem()
        {
            try
            {
                IsRefreshingPersonal = true;
                dynamic result = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuarios");
                foreach (var item in result)
                {
                    Personaltems.Add(new PersonalItem
                    {
                        fotoPerfil = "ic_profile_color.png",
                        idUsuario = item.idUsuario,
                        dni = item.dni,
                        nombresApellidos = item.nombresApellidos.ToString(),
                        usuario = item.usuario,
                        correo = item.correo,
                        celular = item.celular,
                        idCargo = item.idCargo,
                        idCargo_Usuario = item.idCargo_Usuario,
                        cip = item.cip,
                        cargo = item.cargo,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
                return;
            }
            finally
            {
                IsRefreshingPersonal = false;
            }
        }
        #endregion
        protected override bool OnBackButtonPressed()
        {
            App.Navigator.Detail = new NavigationPage(new PaginaUsuario());
            return true;

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
            var items = listViewPersonal.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                // Aqui Logica de programacion cada ves que se ejecute este evento =====================================================//
                // int cargarNuevos = 5; // solo de prueva
                // int totalRegistroActual = PropietarioItems.Count(); // solo de prueva
                // CargarPersonalItem();
            }
        }
        #endregion

    }
}
