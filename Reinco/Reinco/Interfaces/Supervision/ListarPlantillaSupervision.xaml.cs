using Reinco.Entidades;
using Reinco.Interfaces.Plantilla;
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
            directorio.Text = App.directorio + "\\Supervisiones";

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
                IsRefreshingPlantillaSupervision = true;
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idPlantillaPropObra", IdPlantillaObra } };
                dynamic result = await servicio.MetodoGet("ServicioSupervision.asmx", "SupervisionesxIdPlantillaObra", variables);

                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        await DisplayAlert("Supervisiones por plantilla", "No hay supervisiones", "Aceptar");
                        return;
                    }
                    else
                    {
                        foreach (var item in result)
                        {
                            nombreA ="As: "+ item.nombreAsistente+" - ";
                            nombreR ="Resp: "+ item.nombreResponsable;
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
                                numero = item.nroSupervision == null ? 0 : item.nroSupervision,
                                fecha = item.fecha,
                                partidaEvaluada = item.partidaEvaluada,
                                nivel = item.nivel,
                                colorSupervision = Color,
                                idSupervision =item.idSupervision
                            });
                        }
                        // fin del listado
                    }
                }
                else
                {
                    await mensaje.MostrarMensaje("Supervisiones", "Error de respuesta del servicio, Contáctese con el administrador.");
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error:", ex.Message);
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

    }
}
