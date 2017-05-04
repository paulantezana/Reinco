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
            App.directorio = "";
            directorio.Text = App.directorio + "/Personal";
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
                int cargo1 = 0, cargo2 = 0, cargo3 = 0;//gerente,responsable,asistente
                IsRefreshingPersonal = true;
                dynamic result = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuarios");
                foreach (var item in result)
                {
                    string cargos = item.cargos;
                    string[] words = cargos.Split(' ');
                    foreach (string word in words)
                    {
                        if (word == "Asistente")
                            cargo3 = 1;
                        if (word == "Responsable")
                            cargo2 = 1;
                        if (word == "Gerente")
                            cargo1 = 1;
                    }
                    Personaltems.Add(new PersonalItem
                    {
                        fotoPerfil = "ic_profile_color.png",
                    
                        idUsuario = item.idUsuario,
                        dni = item.dni,
                        nombresApellidos = item.nombresApellidos.ToString(),
                        usuario = item.usuario,
                        correo = item.correo,
                        contrasena=item.contrasena,
                        celular = item.celular,
                        idCargo1=cargo1,
                        idCargo2=cargo2,
                        idCargo3=cargo3,
                        cip = item.cip,
                        cargo = item.cargos,
                    });
                    cargo1 = 0;
                    cargo2 = 0;
                    cargo3 = 0;
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
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
        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
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
