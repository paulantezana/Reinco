using Newtonsoft.Json;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarPlantilla : ContentPage
    {
        
        VentanaMensaje dialogService;
        WebService Servicio = new WebService();
        string Mensaje;
        int IdPlantilla;
        public VentanaMensaje mensaje;
        #region ===================== Constructor Sin parametros =================== //
        public AgregarPlantilla()
        {
            InitializeComponent();
            // servicios
            dialogService = new VentanaMensaje();

            // Eventos
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;
        }
        #endregion
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        #region ===================== Agregar PLantillas =====================
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(codPlantilla.Text) || string.IsNullOrEmpty(nombrePlantilla.Text))
                {
                    await dialogService.MostrarMensaje("Agregar plantilla", "Debe rellenar todos los campos.");
                    return;
                }
                object[,] variables = new object[,] { { "codigo", codPlantilla.Text }, { "nombre", nombrePlantilla.Text } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantilla.asmx", "IngresarPlantilla", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Agregar Plantilla", Mensaje, "OK");
                    App.ListarPlantilla.PlantillaItems.Clear();
                    App.ListarPlantilla.CargarPlantilla();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Plantilla", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }

        }
        #endregion

        #region ===================== Constructor con parametros (Actualizar plantilla) ===================== //
        public AgregarPlantilla(object idPlantilla, object codigo, object nombre)
        {
            InitializeComponent();
            IdPlantilla = Convert.ToInt16(idPlantilla);
            codPlantilla.Text = codigo.ToString();
            nombrePlantilla.Text = nombre.ToString();
            guardar.Clicked += Guardar_Clicked1;
            cancelar.Clicked += Cancelar_Clicked1;
        }
        #endregion

        #region==================modificar plantilla================================
        private async  void Guardar_Clicked1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(codPlantilla.Text) || string.IsNullOrEmpty(nombrePlantilla.Text))
                {
                    await dialogService.MostrarMensaje("Modificar plantilla", "Debe rellenar todos los campos.");
                    return;
                }
                object[,] variables = new object[,] { { "idPlantilla", IdPlantilla }, { "codigo", codPlantilla.Text }, { "nombre", nombrePlantilla.Text } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantilla.asmx", "ModificarPlantilla", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Modificar Plantilla", Mensaje, "OK");
                    App.ListarPlantilla.PlantillaItems.Clear();
                    App.ListarPlantilla.CargarPlantilla();
                    await Navigation.PopAsync();
                    return;
                }
              await  Navigation.PopAsync();
                return;
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Modificar Plantilla", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        #endregion
        
        private void Cancelar_Clicked1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
