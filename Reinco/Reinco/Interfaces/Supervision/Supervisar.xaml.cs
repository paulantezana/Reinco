﻿using Reinco.Entidades;
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
    public partial class Supervisar : ContentPage, INotifyPropertyChanged
    {


        public VentanaMensaje mensaje;
        int IdSupervision;


        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();


        new public event PropertyChangedEventHandler PropertyChanged;




        public string notaSupervision { get; set; }
        public bool observacion { get; set; }
        public bool disposicion { get; set; }
        public bool recepcion { get; set; }
        public bool entrega { get; set; }
        public bool conformitad { get; set; }
        public bool isRefreshingSupervisar { get; set; }
        public bool guardarSupervisionIsrunning { get; set; }



        public ObservableCollection<SupervisarActividadItem> SupervisarActividadItems { get; set; }



        #region ============================= Comandos =============================

        public ICommand guargarSupervision { get; private set; }
        public ICommand CancelarSupervision { get; private set; }
        public ICommand RefreshSupervisarCommand { get; private set; }

        #endregion



        #region ============================= Refrescar =============================
        public bool IsRefreshingSupervisar
        {
            set
            {
                if (isRefreshingSupervisar != value)
                {
                    isRefreshingSupervisar = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingSupervisar"));
                }
            }
            get
            {
                return isRefreshingSupervisar;
            }
        }
        public bool GuardarSupervisionIsrunning
        {
            set
            {
                if (guardarSupervisionIsrunning != value)
                {
                    guardarSupervisionIsrunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GuardarSupervisionIsrunning"));
                }
            }
            get
            {
                return guardarSupervisionIsrunning;
            }
        }
        #endregion




        public Supervisar()
        {
            InitializeComponent();
            SupervisarActividadItems = new ObservableCollection<SupervisarActividadItem>();
            CargarSupervisarActividadItem();

            // Guardar Supervision
            guargarSupervision = new Command(() =>
            {
                DisplayAlert("Ok", "Me ejecute", "Aceptar");
            });

            // Navegacion hacia atras Boton Cancelar
            CancelarSupervision = new Command(() =>
            {
                Navigation.PopAsync();
            });

            // Contexto Actual Para los bindings
            this.BindingContext = this;
        }
        public Supervisar(int idSupervision)
        {
            InitializeComponent();
            IdSupervision = idSupervision;
            SupervisarActividadItems = new ObservableCollection<SupervisarActividadItem>();
            CargarSupervisarActividadItem();
            this.BindingContext = this;
        }



        private async void CargarSupervisarActividadItem()
        {
            byte x = 01;
            try
            {
                //object[,] variables = new object[,] { { "IdSupervision", IdSupervision } };
                //dynamic obras = await Servicio.MetodoGet("ServicioSupervision.asmx", "ActividadesxSupervision", variables);
                //foreach (var item in obras)
                //{

                //    SupervisarActividadItems.Add(new SupervisarActividadItem
                //    {
                //        item =x++.ToString(),
                //        actividad =item.nombre,
                //        tolerancia=item.tolerancia_maxima,
                //        observacionLevantada = true,
                //        aprobacion = true,
                //        anotacionAdicinal = "",
                //    });
                //}

                // Iteracion Solo Para Hacer Pruebas sin Web Service
                for (int i = 0; i < 15; i++)
                {
                    SupervisarActividadItems.Add(new SupervisarActividadItem
                    {
                        item = x++.ToString(),
                        actividad = "Nombre de la supervision",
                        tolerancia = "Tolerancia",
                        observacionLevantada = true,
                        aprobacion = true,
                        anotacionAdicinal = "Anotacion adicional",
                    });
                }
                // Fin Iteracion De solo Pruebas
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
           
        }

        
    }
}
