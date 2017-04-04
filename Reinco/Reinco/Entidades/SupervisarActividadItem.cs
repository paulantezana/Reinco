using Newtonsoft.Json;
using Plugin.Media;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class SupervisarActividadItem: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string nombrePropiedad)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombrePropiedad));
        }


        WebService Servicio = new WebService();
        string Mensaje;
        VentanaMensaje mensaje = new VentanaMensaje();

        public int idSupervisionActividad { get; set; }
        public string item { get; set; }
        public string actividad { get; set; }
        public bool observacionLevantada { get; set; }
        public string anotacionAdicinal { get; set; }
        public string tolerancia { get; set; }
        public bool aprobacion { get; set; }


        public ICommand guardarItem { get; private set; }
        public ICommand AprobacionCommand { get; private set; }


        public SupervisarActividadItem()
        {
            AprobacionCommand = new Command(() =>
            {
                App.Current.MainPage.DisplayAlert("Alerta", "Me ejecute", "Aceptar");
            });

            #region Lammando A Guardar Actividad
            guardarItem = new Command(() =>
                {
                // App.Current.MainPage.DisplayAlert("Aceptar", this.actividad + this.anotacionAdicinal, "Aceptar");
                GuardarActividad();
                }); 
            #endregion

            #region ================= Expandir =================
            MostrarAnotacion = false;
            ExpandirAnotacion = new Command(() =>
            {
                if (toggle == 0)
                {
                    MostrarAnotacion = true;
                    toggle = 1;
                }
                else
                {
                    MostrarAnotacion = false;
                    toggle = 0;
                }
            }); 
            #endregion

            #region ================ Uso De La Camara ================
            EncenderCamara = new Command(async () =>
                {
                    await CrossMedia.Current.Initialize(); // Inicializando la libreri

                // Verificando si el dispotivo tiene Camara
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", ":( No hay cámara disponible.", "Aceptar");
                    }

                // Directorio para almacenar la imagen
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "fotos",
                    Name = "fotoreinco.jpg",
                    AllowCropping = true,
                });

                // mostrando la imagen en la interfas del telefono
                if (file != null)
                {
                    await App.Current.MainPage.DisplayAlert("Localizacion Del Archivo", file.Path, "OK");

                    RutaImagen = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                    

                    // Preparando la foto para enviar al webservice
                    var foto = file.GetStream();
                    var fotoArray = ReadFully(foto);

                    var fotos = new Fotos
                    {
                        id = 5,
                        array = fotoArray,
                    };

                    var imagenSerializado = JsonConvert.SerializeObject(fotos);
                    var body = new StringContent(imagenSerializado, Encoding.UTF8, "application/json");
                    var client = new HttpClient();
                    var url = "http://190.42.122.110/";
                    var response = await client.PostAsync(url, body);
                    // Retratado 
                    // var streamTratado = new MemoryStream(fotoArray);
                    // this.ImagenTratado = streamTratado;
                }
                // End Camera
            });
            #endregion

        }

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
        #endregion

        #region ================= Preparando la Interaccion De la Camara =================
        // Preparadndo la imagen para enviar


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


        public byte [] fotoArray { get; set; } // Array De Bits Para Enviar la Foto Al Web Service

        private ImageSource imagenTratado { get; set; }
        public ImageSource ImagenTratado
        {
            set
            {
                if (imagenTratado != value)
                {
                    imagenTratado = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImagenTratado"));
                }
            }
            get
            {
                return imagenTratado;
            }
        }
        // InterAccion Con la Camara
        private ImageSource rutaImagen;
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
        public ICommand EncenderCamara { get; private set; }
        #endregion

        #region ===================== Guardar Actividad =====================
        public async void GuardarActividad()
        {
            try
            {
                int Si, No;
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
                object[,] variables = new object[,] {
                        { "idSupervisionActividad",idSupervisionActividad  } ,{ "si", Si },{ "no", No },
                        { "observacionLevantada", No }, { "anotacionAdicional", anotacionAdicinal }};

                dynamic result = await Servicio.MetodoGetString("SupervisionActividad.asmx", "guardarSupervisionActividad", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Guardar actividad", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Usuario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        } 
        #endregion
        
    }
}
