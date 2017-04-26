using Reinco.Entidades;
using Reinco.Interfaces.Plantilla;
using Reinco.Recursos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
    public partial class ListarPlantillaSupervision : ContentPage, INotifyPropertyChanged
    {

        #region +---- Atributos ----+
        public VentanaMensaje mensaje;
        string nombreA = "";
        string nombreR = "";
        string Cargo = "";
        string Color = "";
        int IdPlantillaObra;
        #endregion

        #region +---- Services ----+
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        #endregion

        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ICommand AgregarSupervision { get; private set; }
        public ICommand generarReporte { get; private set; }

        private bool isRefreshingPlantillaSupervision { get; set; }
        public bool IsRefreshingPlantillaSupervision
        {
            set
            {
                if (isRefreshingPlantillaSupervision != value)
                {
                    isRefreshingPlantillaSupervision = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingPlantillaSupervision"));
                }
            }
            get
            {
                return isRefreshingPlantillaSupervision;
            }
        }

        public ObservableCollection<PlantillaSupervisionItem> PlantillaSupervisionItems { get; set; }

        public ICommand RefreshPlantillaSupervisionCommand { get; private set; }

        #region ========================= Constructor =========================
        public ListarPlantillaSupervision(int idPlantillaObra, int idObra, int idPlantilla, string nombrePlantilla /*= "Supervision"*/)
        {
            InitializeComponent();
            directorio.Text = App.directorio + "/Supervisiones";

            IdPlantillaObra = idPlantillaObra;
            this.Title = nombrePlantilla;
            Cargo = App.cargo;
            PlantillaSupervisionItems = new ObservableCollection<PlantillaSupervisionItem>();
            CargarPlantillaSupervision();
            AgregarSupervision = new Command(() =>
            {
                Navigation.PushAsync(new CrearSupervision(IdPlantillaObra));
            });
            generarReporte = new Command(() =>
              {
                  Navigation.PushAsync(new Reporte(idObra, idPlantilla));
              });
            RefreshPlantillaSupervisionCommand = new Command(() =>
             {
                 PlantillaSupervisionItems.Clear();
                 CargarPlantillaSupervision();
             });

            this.BindingContext = this;
        } 
        #endregion

        private void NuevaSupervision_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #region ================== Cargar Supervisiones =========================
        public async void CargarPlantillaSupervision()
        {
            try
            {
                listaVacia.IsVisible = false;
               // IsRefreshingPlantillaSupervision = true;
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idPlantillaPropObra", IdPlantillaObra } };
                dynamic result = await servicio.MetodoGet("ServicioSupervision.asmx", "SupervisionesxIdPlantillaObra", variables);

                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        listaVacia.IsVisible = true;
                        lblListaVacia.Text = "No hay supervisiones para esta plantilla.";
                        btngenerarReporte.IsEnabled = false;
                        return;
                    }
                    else
                    {
                        foreach (var item in result)
                        {
                            nombreA ="As: "+ item.nombresApellidos+" - ";
                            nombreR ="Resp: "+ item.nombreResponsable;
                            string fechaSt = item.fecha;//convertir a string el json de fecha
                            DateTime fechaS = Convert.ToDateTime(fechaSt);//convertir a datetime el string de la fecha
                            if (item.firma_recepcion==1&&item.firma_notificacion==1&&item.firma_conformidad==1)
                            {
                                Color = "#77FF77";
                            }
                            else
                                Color = "#FF7777";
                            if (Cargo == "Asistente")
                                nombreA = "";
                            if (Cargo == "Responsable")
                                nombreR = "";
                            PlantillaSupervisionItems.Add(new PlantillaSupervisionItem
                            {
                                nombre = nombreA + nombreR,
                                nombreAsistente=item.nombresApellidos,
                                numero = item.nroSupervision == null ? 0 : item.nroSupervision,
                                fecha = fechaS.ToString("dd/M/yyyy", CultureInfo.InvariantCulture),//convertir a date el datetime de fecha
                                partidaEvaluada = item.partidaEvaluada,
                                nivel = item.nivel,
                                colorSupervision = Color,
                                idSupervision =item.idSupervision,
                                correo=item.correo,
                                bloque=item.block
                            });
                            App.correo = item.correo;
                        }
                        // fin del listado
                        App.ultimoNroSupervision = PlantillaSupervisionItems[PlantillaSupervisionItems.Count - 1].numero;
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Supervisiones", "Error de respuesta del servicio, Contáctese con el administrador.", "Aceptar");
                    return;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Supervisiones",ex.Message, "Aceptar");
                return;
            }
            finally
            {
                IsRefreshingPlantillaSupervision = false;
            }
        }
        #endregion

        #region ======================== Global ========================
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarPlantillaSupervision = this;
        }
        #endregion

        #region ================================ Scroll Infinito ================================
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = listViewPlantillaSupervicion.ItemsSource as IList;
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
