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
                        if (value == true)
                        {
                            _aprobacion = false;
                            aprobacion = false;
                        }
                        else
                        {
                            _aprobacion = true;
                            aprobacion = true;
                        }
                        ObservacionLevantada = value;
                        _observacionLevantada = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("observacionLevantada"));
                        GuardarActividad();
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
                    if (value != _aprobacion)
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


        #region ================ Constructor ================
        public SupervisarActividadItem()
        {

            guardarItem = new Command(() =>
            {
                // App.Current.MainPage.DisplayAlert("Aceptar", this.actividad + this.anotacionAdicinal, "Aceptar");
                GuardarActividad();
            });

            guardarIsVisible = false;

            #region ================= Expandir Y Havilitar Boton Guardar =================
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

            #region ================ Uso De La Camara ================
            try
            {
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
                        Name = "fotoreinco.jpg"
                    });

                    // mostrando la imagen en la interfas del telefono
                    if (file != null)
                    {
                        RutaImagen = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            file.Dispose();
                            return stream;
                        });



                        //Preparando la foto para enviar al webservice
                        var foto = file.GetStream();
                        ArrayFotos = ReadFully(foto);
                        var fotos = new Fotos
                        {
                            id = 5,
                            array = fotoArray,
                        };
                        string imagen64 = Convert.ToBase64String(ArrayFotos);
                        object[,] variables = new object[,] {
                            { "idSupervisionActividad",idSupervisionActividad  } ,{ "foto", imagen64 },{ "anotacionAdicional", "asdfasdf" }};

                        dynamic result = await Servicio.MetodoPostStringImagenes("SupervisionActividad.asmx", "guardarAnotacionFoto", variables);
                        Mensaje = Convert.ToString(result);
                        if (result != null)
                        {
                            await App.Current.MainPage.DisplayAlert("Guardar Foto y Anotación", Mensaje, "OK");
                            return;
                        }
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

        //public bool obsLevActivar { get; set; }
        //public bool ObsLevActivar
        //{
        //    set
        //    {
        //        if (obsLevActivar != value)
        //        {
        //            obsLevActivar = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ObsLevActivar"));
        //        }
        //    }
        //    get
        //    {
        //        return obsLevActivar;
        //    }
        //}

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
        public async void GuardarAnotacionFoto()
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
                { "idSupervisionActividad",idSupervisionActividad  } ,{ "foto", base64String },{ "anotacionAdicional", anotacion }};

                dynamic result = await Servicio.MetodoPostStringImagenes("SupervisionActividad.asmx", "guardarAnotacionFoto", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Guardar Foto y Anotación", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Foto y Anotación", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        #endregion

        #region ========================== Guardar Actividad ==========================
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
                string anotacion = anotacionAdicinal;
                if (anotacionAdicinal == null)
                {
                    anotacion = "";
                }
                object[,] variables = new object[,] {
                        { "idSupervisionActividad",idSupervisionActividad  } ,{ "si", Si },{ "no", No },
                        { "observacionLevantada", No }, { "anotacionAdicional", anotacion }};

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
