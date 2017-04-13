using Plugin.Media;
using Reinco.Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FotosxActividad : ContentPage
    {
        public ObservableCollection<FotosxActividadItem> FotosxActividadItems { get; set; }
        public FotosxActividad(/*SupervisarActividadItem Actividad*/)
        {
            InitializeComponent();
            FotosxActividadItems = new ObservableCollection<FotosxActividadItem>();
            CargarFotosxActividad();

            // Sacar Foto
            StackLayout nuevaFoto = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(8, 15, 8, 15)
            };
            nuevaFoto.Children.Add(new Image
            {
                Source = "ic_camara.png",
                HeightRequest = 30,
                WidthRequest = 30,
                VerticalOptions = LayoutOptions.CenterAndExpand
            });
            nuevaFoto.Children.Add(new Label { Text = "Nueva Foto", VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#FFFFFF") });

            // Llamando al comando para llamar la Camara
            nuevaFoto.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await camara();
                    var fotoN = new uiImagen(FotosxActividadItems[FotosxActividadItems.Count() - 1]).layoutImagen;
                    GaleriaContainer.Children.Add(fotoN);
                })
            });

            // Pintando la  Fotos En UI
            GaleriaContainer.Children.Add(nuevaFoto);
            foreach (var FotosxActividad in FotosxActividadItems)
            {
                GaleriaContainer.Children.Add(new uiImagen(FotosxActividad).layoutImagen);
            }
            // End
        }

        #region ===================================== Camara =====================================
        private async Task camara()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            var imageSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            // Agregando Uno Nuevos
            FotosxActividadItems.Add(new FotosxActividadItem
            {
                id = 12,
                foto = file.Path,
                file = file.GetStream()
            });
        } 
        #endregion

        #region ================================= Cargando Las Fotos =================================
        private void CargarFotosxActividad()
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    FotosxActividadItems.Add(new FotosxActividadItem
                    {
                        id = i,
                        foto = "http://lorempixel.com/300/400/"
                    });
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alerta", ex.Message, "Aceptar");
            }
        } 
        #endregion

    }

    #region ===================================== UI Imagen =====================================
    public class uiImagen
    {
        public AbsoluteLayout layoutImagen { get; set; }
        public uiImagen(FotosxActividadItem FotoActividad)
        {
            layoutImagen = new AbsoluteLayout();

            Image imagen = new Image()
            {
                // WidthRequest = 100,
                // HeightRequest = 177,
            };
            imagen.Source = FotoActividad.foto;
            AbsoluteLayout.SetLayoutBounds(imagen, new Rectangle(1, 1, 1, 1));
            AbsoluteLayout.SetLayoutFlags(imagen, AbsoluteLayoutFlags.All);


            Image eliminar = new Image();
            eliminar.Source = "ic_eliminar.png";
            eliminar.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() => { this.eliminarFoto(FotoActividad); })
            });
            AbsoluteLayout.SetLayoutBounds(eliminar, new Rectangle(0, 1, 40, 40));
            AbsoluteLayout.SetLayoutFlags(eliminar, AbsoluteLayoutFlags.PositionProportional);

            Image guardar = new Image();
            guardar.Source = "ic_guardar.png";
            guardar.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() => { this.guardarFoto(FotoActividad); })
            });
            AbsoluteLayout.SetLayoutBounds(guardar, new Rectangle(1, 1, 40, 40));
            AbsoluteLayout.SetLayoutFlags(guardar, AbsoluteLayoutFlags.PositionProportional);

            BoxView estado = new BoxView();

            layoutImagen.Children.Add(imagen);
            layoutImagen.Children.Add(guardar);
            layoutImagen.Children.Add(eliminar);
        }

        private void eliminarFoto(FotosxActividadItem FotoActividad)
        {
            App.Current.MainPage.DisplayAlert("Eliminar", FotoActividad.id.ToString(), "Aceptar");
        }

        private async void guardarFoto(FotosxActividadItem FotoActividad)
        {
            //var nuevo_file = ReducirImagen.ResizeImage();
            var contenido = new MultipartFormDataContent();
            contenido.Add(new StreamContent(FotoActividad.file), "\"file\"", $"\"{FotoActividad.foto}\"");
            /* var values = new[]
             {
                 new KeyValuePair<string, string>("idSupervisionActividad", idSupervisionActividad.ToString()),
             };
             foreach (var keyValuePair in values)
             {
                 contenido.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
             }*/

            var servicioUpload = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/ServicioFoto.asmx/ImagenPost";
            var client = new HttpClient();
            var httpResponserMessage = await client.PostAsync(servicioUpload, contenido);
            string mensajeRespuesta = await httpResponserMessage.Content.ReadAsStringAsync();
            await App.Current.MainPage.DisplayAlert("ERROR:", mensajeRespuesta, "Aceptar");

            // App.Current.MainPage.DisplayAlert("Guardar", FotoActividad.id.ToString(), "Aceptar");
        }
    } 
    #endregion

}
