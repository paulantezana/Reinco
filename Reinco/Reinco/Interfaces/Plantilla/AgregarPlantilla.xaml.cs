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
        // ===================== Constructor Para Crear Plantilla =================== //
        public AgregarPlantilla()
        {
            InitializeComponent();
            // servicios
            dialogService = new VentanaMensaje();

            // Eventos
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;
        }
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
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Plantilla", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }

        }
        #endregion
        // ===================== Constructor Para Actualizar O Cambiar Plantilla ===================== //
        public AgregarPlantilla(object idPlantilla)
        {
            InitializeComponent();
            IdPlantilla = Convert.ToInt16(idPlantilla);
            guardar.Clicked += Guardar_Clicked1;
            cancelar.Clicked += Cancelar_Clicked1;
        }
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
                    return;
                }
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
        // ===================== END
    }
}
