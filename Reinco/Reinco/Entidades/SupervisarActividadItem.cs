using Newtonsoft.Json;
using Plugin.Media;
using Reinco.Interfaces.Supervision;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class SupervisarActividadItem : INotifyPropertyChanged
    {
        WebService Servicio = new WebService();
        string Mensaje;
        VentanaMensaje mensaje = new VentanaMensaje();

        public int idSupervisionActividad { get; set; }
        public string item { get; set; }
        public string actividad { get; set; }
        public string anotacionAdicinal { get; set; }
        public string tolerancia { get; set; }
        public int animacion { get; set; }
        public bool sinFirmaEntrega { get; set; }
        public bool obraActiva { get; set; }
        public bool firmaEntregaNotificacion { get; set; }
        public bool firmaConformidad { get; set; }
        #region ================ Preparando pa mostrar o ocultar el boton guardar ================
        public bool GuardarIsVisible { get; set; }
        public bool guardarIsVisible
        {
            set
            {
                if (GuardarIsVisible != value)
                {
                    GuardarIsVisible = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("guardarIsVisible"));
                }
            }
            get
            {
                return GuardarIsVisible;
            }
        } 
        #endregion

        #region ================ Detectar Cambios y guardar ================
        /// <summary>
        /// 
        /// </summary>
        public bool _observacionLevantada { get; set; }
        public bool ObservacionLevantada { get; set; }
        public bool observacionLevantada
        {
            get
            {
                return ObservacionLevantada;
            }
            set
            {

                if (ObservacionLevantada != value)
                {
                    if (value != _observacionLevantada)
                    {
                        if (firmaConformidad==false)//==cuando la firma de entrega este activada, se desactiva  antes sinfirmaentrega
                        {
                            if (value == true)
                            {
                               // _aprobacion = false;
                               // aprobacion = false;
                            }
                            else
                            {
                               // _aprobacion = true;
                             //   aprobacion = true;
                            }
                            ObservacionLevantada = value;
                            _observacionLevantada = value;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("observacionLevantada"));
                            GuardarActividad();
                        }
                        else// cuando la firma esta enviada, no se puede modificar
                        {

                            if (value)
                            {
                                ObservacionLevantada = false;
                                _observacionLevantada = false;
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("observacionLevantada"));
                            }
                            else
                            {
                                ObservacionLevantada = true ;
                                _observacionLevantada = true;
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("observacionLevantada"));

                            }
                        }
                    }
                    else
                    {
                        ObservacionLevantada = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("observacionLevantada"));
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool _aprobacion { get; set; }
        public bool Aprobacion { get; set; }
        public bool aprobacion
        {
            get
            {
                return Aprobacion;
            }
            set
            {
                if (Aprobacion != value)
                {
                    if (value != _aprobacion)//ha cambiado 
                    {
                        if (sinFirmaEntrega)//==cuando la firma de entrega este activada, se desactiva  
                        {
                            if (value == true)
                            {
                                _observacionLevantada = false;
                                observacionLevantada = false;
                            }
                            else
                            {
                                _observacionLevantada = true;
                                observacionLevantada = true;
                            }
                            Aprobacion = value;
                            _aprobacion = value;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("aprobacion"));

                            GuardarActividad();
                        }
                        else// cuando la firma esta enviada, no se puede modificar
                        {
                            
                            if (value)
                            {
                                Aprobacion = false;
                                _aprobacion = false;
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("aprobacion"));
                            }
                            else
                            {
                                Aprobacion = true;
                                _aprobacion = true;
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("aprobacion"));
                            
                            }
                        }
                    }
                    else
                    {
                        Aprobacion = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("aprobacion"));
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary> 
	#endregion

        public byte[] ArrayFotos { get; set; }

        public ICommand guardarItem { get; private set; }
        public ICommand verFotos { get; private set; }


        #region ================ Constructor ================
        public SupervisarActividadItem()
        {

            guardarItem = new Command(() =>
            {
                // App.Current.MainPage.DisplayAlert("Aceptar", this.actividad + this.anotacionAdicinal, "Aceptar");
                GuardarActividad();
            });
            verFotos = new Command(() =>
            {
                App.Supervisar.Navigation.PushAsync(new FotosxActividad(this));
            });
            guardarIsVisible = false;
            
            #region ================= Expandir Y Habilitar Boton Guardar =================
            MostrarAnotacion = false;
            ExpandirAnotacion = new Command(() =>
            {
                if (toggle == 0)
                {
                    MostrarAnotacion = true;
                    guardarIsVisible = true;
                    toggle = 1;
                }
                else
                {
                    MostrarAnotacion = false;
                    guardarIsVisible = false;
                    toggle = 0;
                }
            });
            #endregion

            #region ================ Uso De La Cámara ================
            try
            {
                EncenderCamara = new Command(async () =>
                {
                    await CrossMedia.Current.Initialize(); // Inicializando la libreria CrossMedia

                    // Verificando si el dispotivo tiene Cámara
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        await App.Current.MainPage.DisplayAlert("Error", ":( No hay cámara disponible.", "Aceptar");

                    // Directorio para almacenar la imagen
                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Simple",//storage/emule
                        Name = "MiImagen.jpg"
                    });

                    // mostrando la imagen en la interfaz del teléfono
                    if (file != null)
                    {
                        RutaImagen = ImageSource.FromStream(() => { return file.GetStream(); });
                        //var nuevo_file = ReducirImagen.ResizeImage();
                        var contenido = new MultipartFormDataContent();
                        contenido.Add(new StreamContent(file.GetStream()), "\"file\"", $"\"{file.Path}\"");
                        /* var values = new[]
                         {
                             new KeyValuePair<string, string>("idSupervisionActividad", idSupervisionActividad.ToString()),
                         };
                         foreach (var keyValuePair in values)
                         {
                             contenido.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                         }*/
                        //http://192.168.1.36:80/reinco/ServicioFoto.asmx/ImagenPost
                        var servicioUpload = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/ServicioFoto.asmx/ImagenPost";
                        var client = new HttpClient();
                        var httpResponserMessage = await client.PostAsync(servicioUpload, contenido);
                        var mensajeRespuesta = await httpResponserMessage.Content.ReadAsStringAsync();
                        /*if (mensajeRespuesta.StatusCode == HttpStatusCode.OK)
                        {
                            RutaImagen = ImageSource.FromStream(() => { return file.GetStream(); });
                        }*/
                    }
                    // End Camera
                });
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            #endregion
        } 
        #endregion


        #region ================ Expadir Actividad Por Cada Items Correspondiente ================
        int toggle = 0;
        public ICommand ExpandirAnotacion { get; private set; }
        public bool mostrarAnotacion { get; set; }
        public bool MostrarAnotacion
        {
            set
            {
                if (mostrarAnotacion != value)
                {
                    mostrarAnotacion = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MostrarAnotacion"));
                }
            }
            get
            {
                return mostrarAnotacion;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region ================= Preparando la Interaccion De la Camara =================
        
        // Convertir en Array de bytes
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        // usada para almacenar array de bytes de la foto
        public byte[] fotoArray { get; set; } // Array De Bits Para Enviar la Foto Al Web Service

        // Preparando la interaccion de la camaron y mostrara la foto en la interfaz
        private ImageSource rutaImagen;
        private Image oculto;

        public ImageSource RutaImagen
        {
            set
            {
                if (rutaImagen != value)
                {
                    rutaImagen = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RutaImagen"));
                }
            }
            get
            {
                return rutaImagen;
            }
        }
        // Comando que enciende la camara
        public ICommand EncenderCamara { get; private set; }
        #endregion

        #region ========================== Guardar Anotacion Y Foto ==========================
        public async void GuardarAnotacion() //sòlo guardarà la anotación, la foto se guardará cada vez que saque una foto
        {
            try
            {

                string anotacion = anotacionAdicinal;
                if (anotacionAdicinal == null)
                {
                    anotacion = "";
                }
                string base64String = Convert.ToBase64String(ArrayFotos);
                object[,] variables = new object[,] {
                { "idSupervisionActividad",idSupervisionActividad  },{ "anotacionAdicional", anotacion }};

                dynamic result = await Servicio.MetodoGet("SupervisionActividad.asmx", "guardarAnotacion", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Guardar Anotación", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Anotación", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        #endregion

        #region ========================== Guardar Actividad ==========================
        public async void GuardarActividad()
        {
            try
            {
               
                int Si, No,ObservacionLevantada;
                if (aprobacion == false)
                {
                    Si = 0;
                    No = 1;
                }
                else
                {
                    Si = 1;
                    No = 0;
                }
                if (observacionLevantada == false)
                    ObservacionLevantada = 0;
                else
                    ObservacionLevantada = 1;
                string anotacion = anotacionAdicinal;
                if (anotacionAdicinal == null)
                {
                    anotacion = "";
                }
                object[,] variables = new object[,] {
                        { "idSupervisionActividad",idSupervisionActividad  } ,{ "si", Si },{ "no", No },
                        { "observacionLevantada", ObservacionLevantada }, { "anotacionAdicional", anotacion }};
                animacion = 1;
                dynamic result = await Servicio.MetodoGetString("SupervisionActividad.asmx", "guardarSupervisionActividad", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                   
                    oculto = new Image();
                    await oculto.FadeTo(1, 4000);
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Supervisión", "Error al actualizar: " + ex.ToString());
            }
        }
        #endregion

    }
}
