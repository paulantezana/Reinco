﻿using Reinco.Entidades;
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


            this.DireccionApp = Application.Current.Properties["direccionApp"] + "\\";
            // Contexto para los bindings
            this.BindingContext = this;
        }
        private async void CargarReporteItem()
        {
            int acumular=0;
            try
            {
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idObra", IdObra } , { "idPlantilla", IdPlantilla } };
                dynamic result = await servicio.MetodoGet("ServicioSupervision.asmx", "EnviarReporte", variables);
                        
                        // listando las obras
                        foreach (var item in result)
                        {
                    acumular = item.incidencia + acumular;
                    ReporteItems.Add(new ReporteItem
                    {
                        descripcion = item.nombre,
                        si = item.siSuma,
                        no = item.negativoSuma,
                        lev = item.observacionSuma,
                        incidencia = item.incidencia,
                        acumulado=acumular,
                            });
                        }
                        // fin del listado
            }
            catch (Exception ex)
            {
                await DisplayAlert("Generar Reporte", ex.Message, "Ok");
            }
        }
    }
}
