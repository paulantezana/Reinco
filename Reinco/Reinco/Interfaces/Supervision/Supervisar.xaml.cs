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
        PlantillaSupervisionItem supervision;
        string Mensaje;
        string cargoUsuario;

        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();


        new public event PropertyChangedEventHandler PropertyChanged;

        public string DireccionApp { get; set; }
        public string tituloSupervisar { get; set; }

        public string notaSupervision { get; set; }
        public bool observacion { get; set; }
        public bool disposicion { get; set; }
        public bool recepcion { get; set; }
        public bool activarRecepcion { get; set; }
        public bool entrega { get; set; }
        public bool activarEntrega { get; set; }
        public bool conformitad { get; set; }
        public bool activarConformidad { get; set; }
        public bool isRefreshingSupervisar { get; set; }
        public bool guardarSupervisionIsrunning { get; set; }
        public int restringir { get; set; }

        public ObservableCollection<SupervisarActividadItem> SupervisarActividadItems { get; set; }

        #region ============================= Comandos =============================

        public ICommand guardarSupervision { get; private set; }
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
        }
        public Supervisar(PlantillaSupervisionItem Supervision)
        {
            InitializeComponent();
            supervision = Supervision;
            tituloSupervisar = Supervision.nombreObra;

            SupervisarActividadItems = new ObservableCollection<SupervisarActividadItem>();
            CargarSupervisarActividadItem();

            // Havilitar Firmas
            activarConformidad = true;
            activarEntrega = true;
            activarRecepcion = true;

            TraerSupervision(Supervision.idSupervision);
            // Valores
            DireccionApp = Application.Current.Properties["direccionApp"].ToString() + "\\Supervisar";
            tituloSupervisar = Application.Current.Properties["direccionApp"].ToString();
            cargoUsuario = App.cargo;
            if (cargoUsuario == "Asistente")
                Sconformidad.IsEnabled = false;
            if (cargoUsuario == "")
                Sentrega.IsEnabled = false;
            if (cargoUsuario == "Responsable")
                Srecepcion.IsEnabled = false;
            if (restringir == 1)
            {
               
            }
            // Comandos
            guardarSupervision = new Command(() =>
            {
                GuardarSupervision();
            });
            CancelarSupervision = new Command(() =>
            {
                Navigation.PopAsync();
            });
            RefreshSupervisarCommand = new Command(() =>
            {
                SupervisarActividadItems.Clear();
                CargarSupervisarActividadItem();
            });

            // Contexto Actual Para los bindings
            this.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Supervisar = this;
        }

        #region =========================== cargar actividades de la supervision ===========================
        private async void CargarSupervisarActividadItem()
        {
            byte x = 01;
            try
            {
                IsRefreshingSupervisar = true;
                object[,] variables = new object[,] { { "IdSupervision", supervision.idSupervision } };
                dynamic obras = await Servicio.MetodoGet("ServicioSupervision.asmx", "ActividadesxSupervision", variables);
                foreach (var item in obras)
                {
                    SupervisarActividadItems.Add(new SupervisarActividadItem
                    {
                        item = x++.ToString(),
                        idSupervisionActividad = item.idSupervision_actividad,
                        actividad = item.nombre,
                        tolerancia = item.tolerancia_maxima,
                        anotacionAdicinal = item.anotacion_adicional,
                        _aprobacion = item.si == 0 ? false : true,
                        aprobacion = item.si == 0 ? false : true,
                        _observacionLevantada = item.observacion_levantada == 0 ? false : true,
                        observacionLevantada = item.observacion_levantada == 0 ? false : true,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                IsRefreshingSupervisar = false;
            }

        }
        #endregion

        #region ================================= Traer Supervision =================================
        public async void TraerSupervision(int idSupervision)
        {
            try
            {
                object[,] variables = new object[,] { { "idSupervision", idSupervision } };
                dynamic supervision = await Servicio.MetodoGet("ServicioSupervision.asmx", "TraerSupervision", variables);
                foreach (var item in supervision)
                {
                    restringir = item.firma_Notificacion;
                    EnotaSupervision.Text = item.notaSupervision == null ? "" : item.notaSupervision;
                    Sobservacion.IsToggled = item.observacion == null ? false : true;
                    Sdisposicion.IsToggled = item.disposicion == 0 ? true : false;
                    Srecepcion.IsToggled = item.firma_Recepcion != 1 ? false : true;
                    Sentrega.IsToggled = item.firma_Notificacion != 1 ? false : true;
                    Sconformidad.IsToggled = item.firma_Conformidad != 1 ? false : true;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error", ex.Message);
            }
        }
        #endregion

        #region ============================== guardar supervision ==============================
        public async void GuardarSupervision()
        {
            try
            {
                cambiarEstado(false);
                
                object[,] variables = new object[,] {
                    { "idSupervision", supervision.idSupervision } ,{ "notaSupervision", notaSupervision==null?"":notaSupervision }, { "observacion", observacion==true?1:0 },
                    { "disposicion", disposicion==true?1:0 }, { "firma_recepcion",recepcion==true?1:0  }, { "firma_entrega", entrega==true?1:0 },
                    { "firma_conformidad", conformitad==true?1:0}};
                dynamic result = await Servicio.MetodoGetString("ServicioSupervision.asmx", "GuardarSupervision", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    cambiarEstado(true);
                    await App.Current.MainPage.DisplayAlert("Guardar Supervision", Mensaje, "Aceptar");
                    return;
                }
            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await App.Current.MainPage.DisplayAlert("Guardar Supervision", "Error en el dispositivo o URL incorrecto: " + ex.Message,"Aceptar");
            }
        }
        #endregion

        #region ============================== Cambiar Estado ==============================
        public void cambiarEstado(bool estado)
        {
            guardar.IsEnabled = estado;
            cancelar.IsEnabled = estado;
            if (estado == true) { GuardarSupervisionIsrunning = false; }
            else { GuardarSupervisionIsrunning = true; }
        }
        #endregion

    }
}
