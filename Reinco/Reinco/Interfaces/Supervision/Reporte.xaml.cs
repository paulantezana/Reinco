using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Reporte : ContentPage
    {

        public string DireccionApp { get; set; }
        public int sumaSi { get; set; }
        public int sumaNo { get; set; }
        public int sumaLev { get; set; }
        public int sumaIncidencia { get; set; }
        public int sumaAcumulado { get; set; }
        public int IdObra { get; set; }
        public int IdPlantilla { get; set; }

        public ObservableCollection<ReporteItem> ReporteItems { get; set; }
        public Reporte()
        {
            InitializeComponent();
            ReporteItems = new ObservableCollection<ReporteItem>();
            CargarReporteItem();

            // Contexto para los bindings
            this.BindingContext = this;
        }
        public Reporte(int idObra, int idPlantilla)
        {
            IdObra = idObra;
            IdPlantilla = idPlantilla;
            InitializeComponent();
            ReporteItems = new ObservableCollection<ReporteItem>();
            CargarReporteItem();

            enviar.Clicked += Enviar_Clicked;
            this.DireccionApp = Application.Current.Properties["direccionApp"] + "/";
            // Contexto para los bindings
            this.BindingContext = this;
        }

        private async void Enviar_Clicked(object sender, EventArgs e)
        {
            try
            {
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idObra", IdObra }, { "idPlantilla", IdPlantilla }, { "email", App.correo } };
                dynamic result = await servicio.MetodoGetString("ServicioSupervision.asmx", "CrearReporte", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    enviar.IsEnabled = false;
                    await DisplayAlert("Enviar Reporte", Mensaje, "Aceptar");
                    return;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Enviar Reporte", "Error de respuesta del servicio, Contáctese con el administrador.", "Aceptar");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Generar Reporte", ex.Message, "Ok");
                return;
            }
        }

        private async void CargarReporteItem()
        {
            int acumular=0;
            int totalSi = 0,totalNo=0,totalObs=0;
            try
            {
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idObra", IdObra } , { "idPlantilla", IdPlantilla }  };
                dynamic result = await servicio.MetodoGet("ServicioSupervision.asmx", "EnviarReporte", variables);
                        
                        // listando las obras
                foreach (var item in result)
                {
                    totalSi = item.siSuma+totalSi;
                    totalNo = item.negativoSuma+totalNo;
                    totalObs = item.observacionSuma+totalObs;
                    acumular = item.incidencia + acumular;
                    ReporteItems.Add(new ReporteItem
                    {
                        descripcion = item.nombre,
                        si = item.siSuma,
                        no = item.negativoSuma,
                        lev = item.observacionSuma,
                        incidencia = item.incidencia+"%",
                        acumulado=acumular.ToString()+"%",
                            });
                        }
                        // fin del listado
            }
            catch (Exception ex)
            {
                await DisplayAlert("Generar Reporte", ex.Message, "Ok");
                return;
            }
            lbltotalSi.Text = totalSi.ToString();
            lbltotalNo.Text = totalNo.ToString();
            lbltotalObs.Text = totalObs.ToString();
        }
    }
}
