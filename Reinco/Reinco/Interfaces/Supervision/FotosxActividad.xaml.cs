using Plugin.Media;
using Reinco.Entidades;
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
    public partial class FotosxActividad : ContentPage
    {
        public ObservableCollection<FotosxActividadItem> FotosxActividadItems { get; set; }
        public Grid grid { get; set; }
        public FotosxActividad(SupervisarActividadItem Actividad)
        {
            InitializeComponent();
            FotosxActividadItems = new ObservableCollection<FotosxActividadItem>();
            CargarFotosxActividad();

            float fotos = FotosxActividadItems.Count;
            float columnas = 3;
            double filas = Math.Ceiling(fotos / columnas);

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
            nuevaFoto.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await camara();
                    var fotoN = new uiImagen(FotosxActividadItems[FotosxActividadItems.Count() - 1]).layoutImagen;
                    Double nFila = Math.Ceiling(FotosxActividadItems.Count() - 1 / columnas);
                    if (nFila > filas)
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        filas += 1;
                    }
                    Double nColumna = Math.Truncate(FotosxActividadItems.Count() - 1 / nFila);
                    await DisplayAlert("info", (FotosxActividadItems.Count() / nFila).ToString(), "ok");
                    grid.Children.Add(fotoN, Convert.ToInt16(nColumna), Convert.ToInt16(nFila - 1));
                })
            });

            grid = new Grid();

            for (int i = 0; i < columnas; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            }
            for (int i = 0; i < filas; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Rellena
            int item = 0;
            for (int y = 0; y < filas; y++)
            {
                for (int x = 0; x < columnas; x++)
                {
                    if (item < fotos)
                    {
                        grid.Children.Add(new uiImagen(FotosxActividadItems[item]).layoutImagen, x, y);
                        item++;
                    }
                }
            }
            GaleriaContainer.Children.Add(nuevaFoto);
            GaleriaContainer.Children.Add(grid);
        }

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
                foto = file.Path
            });
        }

        private void CargarFotosxActividad()
        {
            try
            {
                for (int i = 0; i < 11; i++)
                {
                    FotosxActividadItems.Add(new FotosxActividadItem
                    {
                        id = i,
                        foto = "http://lorempixel.com/150/260/"
                    });
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alerta", ex.Message, "Aceptar");
            }
        }
    }
    public class uiImagen
    {
        public AbsoluteLayout layoutImagen { get; set; }
        public uiImagen(FotosxActividadItem FotoActividad)
        {
            layoutImagen = new AbsoluteLayout();

            Image imagen = new Image()
            {
                WidthRequest = 100,
                HeightRequest = 177,
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

        private void guardarFoto(FotosxActividadItem FotoActividad)
        {
            App.Current.MainPage.DisplayAlert("Guardar", FotoActividad.id.ToString(), "Aceptar");
        }
    }
}
