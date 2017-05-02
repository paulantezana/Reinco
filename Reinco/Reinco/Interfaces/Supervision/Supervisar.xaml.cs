using Android.Content;
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
        string Mensaje;
        string cargoUsuario;
        int propAsistente = 0;//si es asistente, se pone en 1
        int propGerente = 0;
        int propResidente = 0;
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        dynamic Supervision;
        #region======================== atributos=====================
        new public event PropertyChangedEventHandler PropertyChanged;

        public string DireccionApp { get; set; }
        public string tituloSupervisar { get; set; }

        public string notaSupervision { get; set; }

        public bool recepcion { get; set; }
        public bool activarRecepcion { get; set; }
        public bool entrega { get; set; }
        public bool activarEntrega { get; set; }
        public bool conformitad { get; set; }
        public bool activarConformidad { get; set; }
        public bool isRefreshingSupervisar { get; set; }
        public bool guardarSupervisionIsrunning { get; set; }
        public int notificacionEnviada { get; set; }
        int IdSupervision;
        public ObservableCollection<SupervisarActividadItem> SupervisarActividadItems { get; set; }
        public bool disposicion { get; set; }
        public bool _disposicion { get; set; }//para poder restringir el cambio si existe firma
        public bool observacion { get; set; }
        public bool _observacion { get; set; }
        #endregion

        #region ============================= Comandos =============================

        public ICommand guardarSupervision { get; private set; }
       // public ICommand verPdf { get; private set; }
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
           // notificacionEnviada = 1;
           // SupervisarActividadItems = new ObservableCollection<SupervisarActividadItem>();
           // CargarSupervisarActividadItem();
        }
        
        public Supervisar(int idSupervision,string nombreObra)
        {
            InitializeComponent();
            notificacionEnviada = 1;//restringe cambios en los botones si la supervision ya esta entregada
            IdSupervision = idSupervision;
            tituloSupervisar = nombreObra;
            SupervisarActividadItems = new ObservableCollection<SupervisarActividadItem>();
            CargarSupervisarActividadItem();
            if(App.cargo=="Asistente")
            EnotaSupervision.IsEnabled = false;
            activarConformidad = true;
            activarEntrega = true;
            activarRecepcion = true;
                         
            // Valores
            DireccionApp = Application.Current.Properties["direccionApp"].ToString() + "/Supervisar";
            tituloSupervisar = Application.Current.Properties["direccionApp"].ToString();
            cargoUsuario = App.cargo;
            if (cargoUsuario == "Asistente")
                // Sconformidad.IsEnabled = false;
                propAsistente = 1;
            if (cargoUsuario == "Gerente")
                propGerente = 1;
            // Sentrega.IsEnabled = false;
            if (cargoUsuario == "Responsable")
                propResidente = 1;
                //Srecepcion.IsEnabled = false;
            //restriccion de modificaciones en la supervision
            
            // Comandos
            guardarSupervision = new Command(() =>
            {
                GuardarSupervision();
            });
            //verPdf = new Command(() =>
            //{
            //    mostrarPdf(idSupervision);
            //});
            btnverPdf.Clicked += BtnverPdf_Clicked;
            RefreshSupervisarCommand = new Command(() =>
            {
                SupervisarActividadItems.Clear();
                CargarSupervisarActividadItem();
            });

            // Contexto Actual Para los bindings
            this.BindingContext = this;
            notificacionEnviada = 1;
        }

        private async void BtnverPdf_Clicked(object sender, EventArgs e)
        {
            //WebView wv = new WebView();
            //wv.Source = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/pdf/" + "ejemplo.pdf";
            //Content = wv;
            object[,] variables = new object[,] { { "IdSupervision", IdSupervision } };
            Supervision = await Servicio.MetodoGet("ServicioSupervision.asmx", "ConvertirSupervisionPDF", variables);
            if (Supervision == null)
            {
                await DisplayAlert("Supervisión", Supervision.ToString(), "Ok");
            }
            else {
                Device.OpenUri(new Uri("http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/pdf/" + IdSupervision + ".pdf"));
            }
          
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Supervisar = this;
        }

        #region =========================== cargar actividades de la supervision ===========================
        private async void CargarSupervisarActividadItem()
        {
           int x = 01;//contador de numero de items
            try
            {
              // IsRefreshingSupervisar = true;
                object[,] variables = new object[,] { { "IdSupervision", IdSupervision } };
                Supervision = await Servicio.MetodoGet("ServicioSupervision.asmx", "ActividadesxSupervision", variables);
                foreach (var item in Supervision)
                {
                    notificacionEnviada = item.firma_Notificacion != 1 ? 0 : 1;
                    SupervisarActividadItems.Add(new SupervisarActividadItem
                    {

                        item = x++.ToString(),
                        idSupervisionActividad = item.idSupervision_actividad,
                        actividad = item.nombre,
                        tolerancia = item.tolerancia_maxima,
                        anotacionAdicinal = item.anotacion_adicional,
                        _aprobacion = item.si != 1 ? false : true,
                        aprobacion = item.si != 1 ? false : true,
                        _observacionLevantada = item.observacion_levantada != 1 ? false : true,
                        observacionLevantada = item.observacion_levantada != 1 ? false : true,
                        sinFirmaEntrega = notificacionEnviada == 0 ? true : false,
                        firmaEntregaNotificacion = item.firma_Notificacion != 1 ? false : true,//--------verifica firma entrega notificacion true or false
                        firmaConformidad = item.firma_Conformidad != 1 ? false : true//----verifica firma conformidad  true or false
                });
                   // await Task.Delay(100);
                }
                int i = 0;
                foreach (var item in Supervision)
                {
                    i++;
                    
                    EnotaSupervision.Text = item.notaSupervision == null ? "" : item.notaSupervision;
                    Sobservacion.IsToggled = item.observacion != 1 ? false : true;
                    _observacion = item.observacion != 1 ? false : true;
                    Sdisposicion.IsToggled = item.disposicion != 1 ? false : true;
                    _disposicion = item.disposicion != 1 ? false : true;
                    Srecepcion.IsToggled = item.firma_Recepcion != 1 ? false : true;
                    activarRecepcion =item.firma_Recepcion != 1 ? false : true;
                    Sentrega.IsToggled = item.firma_Notificacion != 1 ? false : true;
                    activarEntrega= item.firma_Notificacion != 1 ? false : true;//--------verifica firma entrega notificacion true or false
                    Sconformidad.IsToggled = item.firma_Conformidad != 1 ? false : true;
                    activarConformidad =item.firma_Conformidad != 1 ? false : true; ;//----verifica firma conformidad  true or false
                    fechaFirmaConformidad.Text = item.fecha_firma_conformidad == null ? "" : item.fecha_firma_conformidad;
                    fechaFirmaEntrega.Text = item.fecha_firma_notificacion == null ? "" : item.fecha_firma_notificacion;
                    fechaFirmaRecepcion.Text = item.fecha_firma_recepcion == null ? "" : item.fecha_firma_recepcion;
                    if (i == 1)
                        break;
                }

                if (notificacionEnviada == 1&&activarConformidad==true)
                {
                    Sdisposicion.PropertyChanged += Sdisposicion_PropertyChanged;
                    Sobservacion.PropertyChanged += Sobservacion_PropertyChanged;
                    if (cargoUsuario != "Gerente")
                    {
                        EnotaSupervision.IsEnabled = false;
                        guardar.IsEnabled = false;
                    }
                    
                }
                if (notificacionEnviada == 0)
                    guardar.IsEnabled = true;
                if(propAsistente==1)
                    Srecepcion.PropertyChanged += Srecepcion_PropertyChanged;
                //if(propGerente==1)
                //    //Sentrega.PropertyChanged += Sentrega_PropertyChanged;
                if (propResidente == 1)
                {
                    Sentrega.PropertyChanged += Sentrega_PropertyChanged;
                    Sconformidad.PropertyChanged += Sconformidad_PropertyChanged;
                }
                  
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
                return;
            }
            finally
            {
                IsRefreshingSupervisar = false;
            }

        }

        private void Srecepcion_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Srecepcion.IsToggled = activarRecepcion;
        }

        private void Sentrega_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Sentrega.IsToggled = activarEntrega;
        }
        #region=============si es asistente, evento que no permita cambiar firma de conformidad y entrega
        private void Sconformidad_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Sconformidad.IsToggled = activarConformidad;
           // Sentrega.IsToggled = activarEntrega;
        }
        #endregion
        #endregion
        #region==========================evento que restringe si las firmas estan activadas
        private void Sobservacion_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Sobservacion.IsToggled = _observacion;
        }

        private void Sdisposicion_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Sdisposicion.IsToggled = _disposicion;
        }
        #endregion
        #region ============================== guardar supervision ==============================
        public async void GuardarSupervision()
        {
            try
            {
                cambiarEstado(false);
                
                object[,] variables = new object[,] {
                    { "idSupervision", IdSupervision} ,{ "notaSupervision", notaSupervision==null?"":notaSupervision }, { "observacion", observacion==true?1:0 },
                    { "disposicion", disposicion==true?1:0 }, { "firma_recepcion",recepcion==true?1:0  }, { "firma_entrega", entrega==true?1:0 },
                    { "firma_conformidad", conformitad==true?1:0}};
                dynamic result = await Servicio.MetodoGetString("ServicioSupervision.asmx", "GuardarSupervision", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    cambiarEstado(true);
                    await App.Current.MainPage.DisplayAlert("Guardar Supervision", Mensaje, "Aceptar");
                    App.ListarPlantillaSupervision.PlantillaSupervisionItems.Clear();
                    App.ListarPlantillaSupervision.CargarPlantillaSupervision();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await App.Current.MainPage.DisplayAlert("Guardar Supervision", "Error en el dispositivo o URL incorrecto: " + ex.Message,"Aceptar");
                return;
            }
        }
        #endregion

        #region ============================== Cambiar Estado ==============================
        public void cambiarEstado(bool estado)
        {
            guardar.IsEnabled = estado;
            if (estado == true) { GuardarSupervisionIsrunning = false; }
            else { GuardarSupervisionIsrunning = true; }
        }
        #endregion
        public async void mostrarPdf(int idAsistente)
        {
            try
            {
                //FotosxActividadItems.Clear();
                //object[,] variables = new object[,] { { "idSupervision", IdSupervision } };
                //dynamic result = await Servicio.MetodoGet("ServicioSupervision.asmx", "MostrarFotos", variables);
                //foreach (var item in result)
                //{
                //    //FotosxActividadItems.Add(new FotosxActividadItem
                //    //{
                //    //    id = item.idFoto,
                //    //    foto = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/fotos/" + item.foto
                //    //    //foto = "http://190.117.145.7/reinco_pruebas_code/fotos/jackeline.jpg"
                //    //});
                //    string pdf = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/pdf/" + IdSupervision+".pdf";

                //}
                //Android.Net.Uri uri = Android.Net.Uri.Parse("file:///" + filePath);
                //Intent intent = new Intent(Intent.ActionView);
                //intent.SetDataAndType(uri, "application/pdf");
                //intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

                //try
                //{
                //    Xamarin.Forms.Context.StartActivity(intent);
                //}
                //catch (Exception)
                //{
                //    Toast.MakeText(Xamarin.Forms.Context, "No Application Available to View PDF", ToastLength.Short).Show();
                //}
                WebView wv = new WebView();
                wv.Source = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/pdf/" +"ejemplo.pdf";
                Content = wv;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.Message, "Aceptar");
            }
        }
    }
}
