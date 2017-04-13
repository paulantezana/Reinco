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

        private bool isRefreshingPropietario { get; set; }

        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IsRefreshingPropietario
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
            directorio.Text = App.directorio + "\\Propietario";
            PropietarioItems = new ObservableCollection<PropietarioItem>();
            CargarPropietarioItem();

            RefreshPropietarioCommand = new Command(() =>
            {
                PropietarioItems.Clear();
                CargarPropietarioItem();
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

        #region ============= Cargar Propietarios==================
        public async void CargarPropietarioItem()
        {
            try
            {
                IsRefreshingPropietario = true;
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
            finally
            {
                IsRefreshingPropietario = false;
            }
        }
        #endregion
    }
}
