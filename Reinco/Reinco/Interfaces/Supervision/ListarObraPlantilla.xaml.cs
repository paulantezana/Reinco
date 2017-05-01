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
            directorio.Text = App.directorio +  "/Plantillas";

            ObraPlantillaItems = new ObservableCollection<ObraPlantillaItem>();
            
            if(App.cargo=="Asistente")
                CargarPlantillaObraSupervisor();
            else
                CargarPlantillaObra();
            // comandos
            RefreshObraPlantillaCommand = new Command(() =>
            {
                ObraPlantillaItems.Clear();
                if (App.cargo == "Asistente")
                    CargarPlantillaObraSupervisor();
                else
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
            directorio.Text = App.directorio + "/Plantillas";

            
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
               listaVacia.IsVisible = false;
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idObra", IdObra } };
                dynamic result = await servicio.MetodoGet("ServicioPlantillaPropietarioObra.asmx", "MostrarPlantillaxidObra", variables);
                string Color = "";
                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        listaVacia.IsVisible = true;
                        lblListaVacia.Text = "No hay plantillas";
                    }
                    else
                    {
                        // listando las obras
                        foreach (var item in result)
                        {
                            if (item.totalSupervisiones == 0)
                                Color = "#dd2400";//si no hay supervisiones, muestra de color rojo
                            else {
                                if (item.totalSupervisiones == item.firmasCompletas)
                                    Color = "#38b503";//si hay supervisiones pero con todas sus firmas, entonces el color es verde
                                else
                                    Color = "#ff9926";//si hay supervisiones incompletas, muestra de color ambar
                            }
                           
                            ObraPlantillaItems.Add(new ObraPlantillaItem
                            {
                                nombre = item.nombre,
                                codigo = item.codigo,
                                idPlantillaObra = item.idPlantilla_Propietario_obra,
                                idObra = item.idObra,
                                idPlantilla = item.idPlantilla,
                                colorPlantilla = Color,
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
        #region==============plantillas por supervisor========================
        public async void CargarPlantillaObraSupervisor()
        {
            try
            {
                //IsRefreshingObraPlantilla = true;
                listaVacia.IsVisible = false;
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idObra", IdObra }, { "idSupervisor", App.idUsuarioAsistente } };
                dynamic result = await servicio.MetodoGet("ServicioPlantillaPropietarioObra.asmx", "MostrarPlantillaxidObraSupervisor", variables);
                string Color = "";
                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        listaVacia.IsVisible = true;
                        lblListaVacia.Text = "No hay plantillas";
                    }
                    else
                    {
                        // listando las obras
                        foreach (var item in result)
                        {
                            if (item.totalSupervisiones == 0)
                                Color = "#dd2400";//si no hay supervisiones, muestra de color rojo
                            else
                            {
                                if (item.totalSupervisiones == item.firmasCompletas)
                                    Color = "#38b503";//si hay supervisiones pero con todas sus firmas, entonces el color es verde
                                else
                                    Color = "#ff9926";//si hay supervisiones incompletas, muestra de color ambar
                            }

                            ObraPlantillaItems.Add(new ObraPlantillaItem
                            {
                                nombre = item.nombre,
                                codigo = item.codigo,
                                idPlantillaObra = item.idPlantilla_Propietario_obra,
                                idObra = item.idObra,
                                idPlantilla = item.idPlantilla,
                                colorPlantilla = Color,
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
        #endregion
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

        public void OnActivitySelected(object o, ItemTappedEventArgs e)
        {
            App.ListarObra.Navigation.PushAsync(new ListarObraPlantilla(obra));
        }

    }
}
