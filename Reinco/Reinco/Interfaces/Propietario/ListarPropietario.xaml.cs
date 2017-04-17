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
               @ por el momento no funcion correctamente
                ================================
                       PAGINACION
                ================================
             */
            //int paginaActual = 5; 
            //int paginas = 3;

            //var anterior = new uiPage("< Anterior");
            //anterior.evento.Command = new Command(() =>
            //{
            //    if (paginaActual > 1)
            //    {
            //        // CargarPropietarioItem(paginaActual - 1);
            //    }
            //});
            //paginacion.Children.Add(anterior.contenedor); // Pintando En La Interfas ===========================|

            //for (int i = 1; i < paginas; i++)
            //{
            //    uiPage page = new uiPage(i.ToString());
            //    page.evento.Command = new Command(() =>
            //    {
            //        DisplayAlert("Info", i.ToString(), "OK");
            //    });
            //    paginacion.Children.Add(page.contenedor);
            //}

            //var siguiente = new uiPage("Siguinete");
            //siguiente.evento.Command = new Command(() =>
            //{
            //    if(paginaActual < paginas)
            //    {
            //        // CargarPropietarioItem(paginaActual + 1);
            //    }
            //});
            //paginacion.Children.Add(siguiente.contenedor); // Pintando En La Interfas ===========================|

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
    }
    public class uiPage
    {
        public StackLayout contenedor { get; set; }
        public TapGestureRecognizer evento { get; set; }
        public uiPage(string texto, string colorFondo = "#EFEFEF",string colorTexto = "#7777")
        {
            contenedor = new StackLayout() {
                BackgroundColor = Color.FromHex(colorFondo)
            };
            contenedor.Padding = new Thickness(5);
            
            contenedor.Children.Add(new Label()
            {
                Text = texto,
                TextColor = Color.FromHex(colorTexto)
            });
            evento = new TapGestureRecognizer();
            contenedor.GestureRecognizers.Add(this.evento);
        }
    }
}
