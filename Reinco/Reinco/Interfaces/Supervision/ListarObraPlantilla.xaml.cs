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

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarObraPlantilla : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged; // Usada paralos refrescar las listas
        string Mensaje;
        ObraItem obra;
        int IdObra;
        public VentanaMensaje mensaje;
        private bool isRefreshingObraPlantilla { get; set; }

        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();

        public bool IsRefreshingObraPlantilla
        {
            set
            {
                if (isRefreshingObraPlantilla != value)
                {
                    isRefreshingObraPlantilla = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingObraPlantilla"));
                }
            }
            get
            {
                return isRefreshingObraPlantilla;
            }
        }

        public ObservableCollection<ObraPlantillaItem> ObraPlantillaItems { get; set; }

        public ICommand RefreshObraPlantillaCommand { get; private set; }
        public ICommand asignarPlantilla { get; private set; }

        public ListarObraPlantilla(ObraItem Obra)
        {
            InitializeComponent();
            obra = Obra;
            IdObra = obra.idObra;
            this.Title = Obra.nombre;
            directorio.Text = App.directorio +  "\\Plantillas";

            ObraPlantillaItems = new ObservableCollection<ObraPlantillaItem>();
            CargarPlantillaObra();
            
            // comandos
            RefreshObraPlantillaCommand = new Command(() =>
            {
                ObraPlantillaItems.Clear();
                CargarPlantillaObra();
            });
            string cargo = App.cargo;
            
            asignarPlantilla = new Command(() =>
            {
                if(cargo!="Asistente")
                    Navigation.PushAsync(new AsignarPlantilla(Obra));
            });
            
            // Contexto para los bindings
            this.BindingContext = this;
        }
        public ListarObraPlantilla(int idObra)
        {
            InitializeComponent();
            ObraPlantillaItems = new ObservableCollection<ObraPlantillaItem>();
            // obra = nombreObra;
            IdObra = idObra;
            this.Title = "asdf";
            directorio.Text = App.directorio + "\\Plantillas";

            
           // CargarPlantillaObra();

            // comandos
            RefreshObraPlantillaCommand = new Command(() =>
            {
                ObraPlantillaItems.Clear();
                CargarPlantillaObra();
            });
            asignarPlantilla = new Command(() =>
            {
                //Navigation.PushAsync(new AsignarPlantilla(Obra));
            });

            // Contexto para los bindings
            this.BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarObraPlantilla = this;
        }
        public async void CargarPlantillaObra()
        {
            try
            {
                //IsRefreshingObraPlantilla = true;
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idObra", IdObra } };
                dynamic result = await servicio.MetodoGet("ServicioPlantillaPropietarioObra.asmx", "MostrarPlantillaxidObra", variables);

                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        //await mensaje.MostrarMensaje("Mostrar Obra Plantilla", "No hay plantillas que mostrar");
                        await DisplayAlert("Información", "No hay plantillas", "Aceptar");
                        return;
                    }
                    else
                    {
                        // listando las obras
                        foreach (var item in result)
                        {
                            ObraPlantillaItems.Add(new ObraPlantillaItem
                            {
                                nombre = item.nombre,
                                codigo = item.codigo,
                                idPlantillaObra = item.idPlantilla_Propietario_obra,
                                idObra = item.idObra,
                                idPlantilla = item.idPlantilla,
                            });
                        }
                        // fin del listado
                    }
                }
                else
                {
                    await mensaje.MostrarMensaje("Iniciar Sesión", "Error de respuesta del servicio, Contáctese con el administrador.");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error:", ex.Message);
                return;
            }
            finally
            {
                IsRefreshingObraPlantilla = false;
            }
        }

        #region ================================ Scroll Infinito ================================
        
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = listViewObraPlantilla.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                // Aqui Logica de programacion cada ves que se ejecute este evento =====================================================//
                // int cargarNuevos = 5; // solo de prueva
                // int totalRegistroActual = PropietarioItems.Count(); // solo de prueva

            }
        }
        #endregion

    }
}
