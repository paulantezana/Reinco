using Reinco.Entidades;
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


        public int sumaSi { get; set; }
        public int sumaNo { get; set; }
        public int sumaLev { get; set; }
        public int sumaIncidencia { get; set; }
        public int sumaAcumulado { get; set; }

        public ObservableCollection<ReporteItem> ReporteItems { get; set; }
        public Reporte()
        {
            InitializeComponent();
            ReporteItems = new ObservableCollection<ReporteItem>();
            CargarReporteItem();

            // Contexto para los bindings
            this.BindingContext = this;
        }
        private async void CargarReporteItem()
        {
            try
            {
                this.sumaSi = 0;
                this.sumaNo = 0;
                this.sumaLev = 0;
                this.sumaIncidencia = 0;
                this.sumaAcumulado = 0;
                for (int i = 0; i < 15; i++)
                {
                    ReporteItems.Add(new ReporteItem
                    {
                        item = i,
                        descripcion = "Recubrimiento de acero",
                        si = 120,
                        no = 500,
                        lev = 15,
                        acumulado = 150,
                        incidencia = 1202,
                        
                    });
                    this.sumaSi += 20;
                    this.sumaNo += 8;
                    this.sumaLev += 15;
                    this.sumaIncidencia += 12;
                    this.sumaAcumulado += 8;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "Aceptar");
            }
        }
    }
}
