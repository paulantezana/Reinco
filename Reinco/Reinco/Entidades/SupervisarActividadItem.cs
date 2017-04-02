using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class SupervisarActividadItem : INotifyPropertyChanged
    {
        public string item { get; set; }
        public string actividad { get; set; }
        public bool aprobacion { get; set; }
        public bool observacionLevantada { get; set; }
        public string anotacionAdicinal { get; set; }
        public string tolerancia { get; set; }



        public ICommand guardarItem { get; private set; }


        public SupervisarActividadItem()
        {
            guardarItem = new Command(() =>
            {
                App.Current.MainPage.DisplayAlert("Aceptar", this.actividad + this.anotacionAdicinal, "Aceptar");
            });


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
            // Camara
            EncenderCamara = new Command(async() =>
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
                }
                // End Camera
            });

        }




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




        // CAMARA 
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
    }
}
