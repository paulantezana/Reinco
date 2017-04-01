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

namespace Reinco.Interfaces.Personal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarPersonal : ContentPage, INotifyPropertyChanged
    {
        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;


        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion




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
        
        public ListarPersonal()
        {
            InitializeComponent();
            Personaltems = new ObservableCollection<PersonalItem>();
            CargarPersonalItem();

            RefreshPersonalCommand = new Command(() =>
            {
                Personaltems.Clear();
                CargarPersonalItem();
                IsRefreshingPersonal = false;
            });
            AgregarPersonal = new Command(() =>
            {
                Navigation.PushAsync(new AgregarPersonal());
            });

            this.BindingContext = this;
        }



        #region Propiedad Global De Esta Pagina
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarPersonal = this;
        } 
        #endregion



        #region +--- comandos ----+
        public ICommand RefreshPersonalCommand { get; private set; }
        public ICommand AgregarPersonal { get; private set; }
        #endregion



        #region==================cargar usuarios==============================
        public async void CargarPersonalItem()
        {
            try
            {
                dynamic result = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuarios");
                foreach (var item in result)
                {
                    Personaltems.Add(new PersonalItem
                    {
                        fotoPerfil = "ic_profile_color.png",
                        idUsuario = item.idUsuario,
                        nombresApellidos = item.nombresApellidos.ToString(),
                        cip = item.cip
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion


        #region=======================eliminar obra====================================
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {
                var idUsuario = ((MenuItem)sender).CommandParameter;
                int IdUsuario = Convert.ToInt16(idUsuario);
                bool respuesta = await DisplayAlert("Eliminar", "Eliminar IdUsuario = " + IdUsuario, "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idUsuario", IdUsuario } };
                dynamic result = await Servicio.MetodoGetString("ServicioUsuario.asmx", "EliminarUsuario", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Usuario", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Usuario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        #endregion
        #region ===================// Modificar Obra CRUD //====================
        public void actualizar(object sender, EventArgs e)
        {
            var idUsuario = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new AgregarPersonal(idUsuario));
        }
        #endregion

    }
}
