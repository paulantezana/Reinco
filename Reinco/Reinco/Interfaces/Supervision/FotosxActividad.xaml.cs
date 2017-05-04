using Plugin.Media;
using Plugin.Media.Abstractions;
using Reinco.Entidades;
using Reinco.Recursos;
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
        private MediaFile file;
        SupervisarActividadItem actividad; // Objeto SupervisarActividadItem
        int idSupervisionActividad;
        
        WebService Servicio = new WebService();
        public FotosxActividad()
        {
            InitializeComponent();
        }
        public FotosxActividad(SupervisarActividadItem Actividad)
        {
            InitializeComponent();
            this.actividad = Actividad; // Almacenando el objeto SupervisarActividadItem para usar en esta interfas
            idSupervisionActividad = actividad.idSupervisionActividad;
            App.idSupervisionActividadE = idSupervisionActividad;
            FotosxActividadItems = new ObservableCollection<FotosxActividadItem>();
            DibujarInterfaz();
        }
        public async void DibujarInterfaz()
        {
            await CargarFotosxActividad(idSupervisionActividad);
            GaleriaContainer.Children.Clear();
            // INICIO -- Agrega interfaz para el  botòn "Agregar Foto"
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
            nuevaFoto.Children.Add(new Label { Text = "Agregar Foto", VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#FFFFFF") });
            // FIN -- Agrega interfaz para el  botòn "Agregar Foto"

            // Al Botón Agregar Foto le agregamos el evento: Llamando al comando para llamar la Camara
            nuevaFoto.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await camara();
                })
            });

            GaleriaContainer.Children.Add(nuevaFoto);
            // Pintando la  Fotos En UI
            foreach (var FotosxActividad in FotosxActividadItems)
            {
                GaleriaContainer.Children.Add(new uiImagen(FotosxActividad).layoutImagen);
            }
        }

        #region ===================================== Camara =====================================
        private async Task camara()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Error con la Cámara", "Cámara no Disponible o no soportada.", "OK");
                return;
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Supervisar",
                Name = "supervision.jpg",
                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = 10
            });

            if (file == null)
                return;
            try {
                
                await guardarFoto();
                file.Dispose();
                DibujarInterfaz();
            }
            catch (Exception ex) {
               await App.Current.MainPage.DisplayAlert("Layout", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
                return;
            }
             
            return;
           
        }
        #endregion

        #region ================================= Cargando Las Fotos =================================
        public async Task CargarFotosxActividad(int idsupActividad)
        {
            try
            {
                FotosxActividadItems.Clear();
                object[,] variables = new object[,] { { "idActividad", idsupActividad } };
                dynamic result = await Servicio.MetodoGet("ServicioFoto.asmx", "MostrarFotos",variables);
                foreach (var item in result)
                {
                    FotosxActividadItems.Add(new FotosxActividadItem
                    {
                        id = item.idFoto,
                        foto = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/fotos/" + item.foto
                        //foto = "http://190.117.145.7/reinco_pruebas_code/fotos/jackeline.jpg"
                    });

                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
            }
        }
        #endregion
        private async Task guardarFoto()
        {
            try
            {
                //Si se quería reducir byte se usaba dependencia de servicios, ya no es necesario
                var contenido = new MultipartFormDataContent();
                contenido.Add(new StreamContent(file.GetStream()), "\"file\"", $"\"{file.Path}\"");
                contenido.Add(new StringContent(idSupervisionActividad.ToString()), "idSupervisionActividad");

                var servicioUpload = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/ServicioFoto.asmx/ImagenPost";
                var client = new HttpClient();
                var httpResponserMessage = await client.PostAsync(servicioUpload, contenido);
                string mensajeRespuesta = await httpResponserMessage.Content.ReadAsStringAsync();
                if ((int)httpResponserMessage.StatusCode != 200)
                    await App.Current.MainPage.DisplayAlert("Resultado:", "Sucedió un Error al intentar subir la última imagen, inténtelo luego", "Aceptar");
                // App.Current.MainPage.DisplayAlert("Guardar", servicioUpload.ToString(), "Aceptar");
            }
            catch (Exception errr)
            {
                await App.Current.MainPage.DisplayAlert("Guardar", "Contacte con el Administrador y reporte el siguiente error:" + errr.ToString(), "Aceptar");
            }
        }

    }


    #region ===================================== UI Imagen =====================================
    public class uiImagen
    {
        FotosxActividad foto = new FotosxActividad();
        public AbsoluteLayout layoutImagen { get; set; }
        WebService Servicio = new WebService();
        public uiImagen(FotosxActividadItem FotoActividad)
        {
            try
            {
                layoutImagen = new AbsoluteLayout();

                Image imagen = new Image()
                {
                    WidthRequest = 250,
                    HeightRequest = 370,
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
                BoxView estado = new BoxView();

                layoutImagen.Children.Add(imagen);
                layoutImagen.Children.Add(eliminar);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Layout", ex.Message, "Aceptar");
                return;
            }
           
        }

        public async void eliminarFoto(FotosxActividadItem FotoActividad)
        {
           await App.Current.MainPage.DisplayAlert("Eliminar", "¿Desea eliminar esta foto?", "Aceptar");
            try
            {
                string Mensaje;
                object[,] variables = new object[,] { { "idFoto", FotoActividad.id } };
                dynamic result = await Servicio.MetodoGetString("ServicioFoto.asmx", "EliminarFoto", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Foto", Mensaje, "OK");
                    await CargarFotosxActividad(App.idSupervisionActividadE);
                    return;
                }
            }
            catch (Exception ex)
            {
              await  App.Current.MainPage.DisplayAlert("Eliminar", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
                return;
            }
            

            return;
        }
        #region ================================= Cargando Las Fotos =================================
        public async Task CargarFotosxActividad(int idsupActividad)
        {
            try
            {
                foto.FotosxActividadItems.Clear();
                object[,] variables = new object[,] { { "idActividad", idsupActividad } };
                dynamic result = await Servicio.MetodoGet("ServicioFoto.asmx", "MostrarFotos", variables);
                foreach (var item in result)
                {
                    foto.FotosxActividadItems.Add(new FotosxActividadItem
                    {
                        id = item.idFoto,
                        foto = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta + "/fotos/" + item.foto
                        //foto = "http://190.117.145.7/reinco_pruebas_code/fotos/jackeline.jpg"
                    });

                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Eliminar", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
            }
        }
        #endregion

    }
    #endregion

}