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

namespace Reinco.Interfaces.Propietario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarPropietario : ContentPage, INotifyPropertyChanged
    {
        protected override bool OnBackButtonPressed()
        {
            App.Navigator.Detail = new NavigationPage(new PaginaUsuario());
            return true;

        }
        WebService Servicio = new WebService();
        int ultimoId = 100000;
        private bool isRefreshingPropietario { get; set; }
        int nroElementos = App.nroElementos;
        new public event PropertyChangedEventHandler PropertyChanged;

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

        public ICommand RefreshPropietarioCommand { get; private set; }
        public ICommand AgregarPropietario { get; private set; }

        #region=============constructor vacio======================
        public ListarPropietario()
        {
            InitializeComponent();
            App.directorio = "";
            directorio.Text = App.directorio + "/Propietario";
            PropietarioItems = new ObservableCollection<PropietarioItem>();
            CargarPropietarioItem(nroElementos, ultimoId);

            RefreshPropietarioCommand = new Command(() =>
            {
                ultimoId = 100000;
                PropietarioItems.Clear();
                CargarPropietarioItem(nroElementos, ultimoId);
            });
            AgregarPropietario = new Command(() =>
            {
                Navigation.PushAsync(new AgregarPropietario());
            });

            // Contexto para los bindings
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
        public async void CargarPropietarioItem(int nroEleiementos,int UltimoId)
        {
            try
            {
               //await DisplayAlert("", "", "OK");
                IsRefreshingPropietario = true;
                object[,] variables = new object[,] { { "nroElementos", nroEleiementos }, { "ultimoId", UltimoId } };
                dynamic result = await Servicio.MetodoGet("ServicioPropietario.asmx", "MostrarPropietarios",variables);
                
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
                return;
            }
            finally
            {
                IsRefreshingPropietario = false;
            }
            ultimoId = PropietarioItems[PropietarioItems.Count - 1].idPropietario;
        }
        #endregion

        #region ================================ Scroll Infinito ================================
      
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = listarPropietario.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                CargarPropietarioItem(nroElementos, ultimoId);//------el ultimo id que se recoge
            }
        } 
        #endregion

    }
   
    #region ============================== UI Paginas ==============================
    public class uiPage
    {
        public StackLayout contenedor { get; set; }
        private int pagina { get; set; }
        public uiPage(string texto, int pagina, string colorFondo = "#EFEFEF", string colorTexto = "#7777")
        {
            this.pagina = pagina;
            contenedor = new StackLayout()
            {
                BackgroundColor = Color.FromHex(colorFondo)
            };
            contenedor.Padding = new Thickness(5);

            contenedor.Children.Add(new Label()
            {
                Text = texto,
                TextColor = Color.FromHex(colorTexto)
            });
            evento();
        }
        public void evento()
        {
            contenedor.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    App.Current.MainPage.DisplayAlert("Info", pagina.ToString(), "Aceptar");
                    // App.ListarPropietarios.CargarPropietarioItem();
                })
            });
        }
    }
    #endregion
   
}
