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

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Supervisar : ContentPage, INotifyPropertyChanged
    {


        public VentanaMensaje mensaje;
        int IdSupervision;
        string Mensaje;

        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();


        new public event PropertyChangedEventHandler PropertyChanged;

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
        public Supervisar(int idSupervision)
        {
            InitializeComponent();
            IdSupervision = idSupervision;
            SupervisarActividadItems = new ObservableCollection<SupervisarActividadItem>();
            CargarSupervisarActividadItem();

            // Guardar Supervision
            guardarSupervision = new Command(() =>
            {
                GuardarSupervision();
            });

            // Navegacion hacia atras Boton Cancelar
            CancelarSupervision = new Command(() =>
            {
                Navigation.PopAsync();
            });

            // Contexto Actual Para los bindings
            this.BindingContext = this;
        }
        #region================cargar actividades de la supervision===========================
        private async void CargarSupervisarActividadItem()
        {
            byte x = 01;
            try
            {
                object[,] variables = new object[,] { { "IdSupervision", IdSupervision } };
                dynamic obras = await Servicio.MetodoGet("ServicioSupervision.asmx", "ActividadesxSupervision", variables);
                foreach (var item in obras)
                {

                    SupervisarActividadItems.Add(new SupervisarActividadItem
                    {
                        item = x++.ToString(),
                        idSupervisionActividad=item.idSupervision_actividad,
                        actividad = item.nombre,
                        tolerancia = item.tolerancia_maxima,
                        observacionLevantada = item.observacion_levantada==0?false:true,
                        aprobacion = item.si==0?false:true,
                        anotacionAdicinal =item.anotacion_adicional,
                    });
                }


            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
           
        }
        #endregion

        #region==================guardar supervision============================
        public async void GuardarSupervision()
        {
            try
            {
                object[,] variables = new object[,] {
                    { "idSupervision", IdSupervision } ,{ "notaSupervision", notaSupervision }, { "observacion", observacion==true?1:0 },
                    { "disposicion", disposicion==true?1:0 }, { "firma_recepcion",recepcion==true?1:0  }, { "firma_entrega", entrega==true?1:0 },
                    { "firma_conformidad", conformitad==true?1:0}};
                dynamic result = await Servicio.MetodoGetString("ServicioSupervision.asmx", "GuardarSupervision", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Guardar Supervision", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Guardar Supervision", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        #endregion
    }
}
