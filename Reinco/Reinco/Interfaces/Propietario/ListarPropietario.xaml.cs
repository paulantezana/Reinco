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
        WebService Servicio = new WebService();

        private bool isRefreshingPropietario { get; set; }
        
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
            directorio.Text = App.directorio + "\\Propietario";
            PropietarioItems = new ObservableCollection<PropietarioItem>();
            CargarPropietarioItem();

            /*
               @ Se usa la uiPage para los eventos y dibujar la interfas
                ================================
                       PAGINACION
                ================================
             */
            int paginas = 3;
            int paginaActual = 2;

            paginacion.Children.Add(new uiPage("< Anterior",paginaActual - 1).contenedor);
            for (int i = 1; i <= paginas; i++)
            {
                if(i == paginaActual)
                {
                    paginacion.Children.Add(new uiPage(i.ToString(), i,"#EFEFEF", "#2196F3").contenedor);
                }
                else
                {
                    paginacion.Children.Add(new uiPage(i.ToString(), i).contenedor);
                }
            }
            paginacion.Children.Add(new uiPage("< Siguiente", paginaActual + 1).contenedor);

            /*
                 @
                ================================
                      FIN PAGINACION
                ================================
             */

            // Comandos
            RefreshPropietarioCommand = new Command(() =>
            {
                PropietarioItems.Clear();
                CargarPropietarioItem();
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

        #region ================================ Scroll Infinito ================================
        /*
            @ Evento que se dispara cadaves que el escroll lega al final de ventana
            ================================
                    SCROLL INFINITO
            ================================
        */
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = listarPropietario.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                // Aqui Logica de programacion cada ves que se ejecute este evento =====================================================//
                // int cargarNuevos = 5; // solo de prueva
                // int totalRegistroActual = PropietarioItems.Count(); // solo de prueva
                // CargarPropietarioItem();
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
