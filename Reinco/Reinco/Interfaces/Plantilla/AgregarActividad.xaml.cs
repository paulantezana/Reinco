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
    public partial class AgregarActividad : ContentPage
    {
        int IdPlantilla;
        int IdActividad;
        WebService Servicio = new WebService();
        string Mensaje;
        public VentanaMensaje mensaje;
        public AgregarActividad()
        {
            InitializeComponent();

            // Eventos
            cancelar.Clicked += Cancelar_Clicked;
        }

        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        #region ===================// Constructor Agregar actividad // ===================//
        public  AgregarActividad(object idPlantilla)
        {
            InitializeComponent();
            guardar.Clicked +=Agregar_Clicked;
            guardar.Text = "Guardar Cambios";
            IdPlantilla = Convert.ToInt16(idPlantilla);
            // eventos
            guardar.Clicked += GuardarCambios_Clicked;
            cancelar.Clicked += Cancelar_Clicked1;
        }
        #endregion
        #region ===================// Constructor Modificar actividad // ===================//
        public AgregarActividad(object idActividad,object idPlantilla)
        {
            InitializeComponent();
            IdPlantilla = Convert.ToInt16(idPlantilla);
            IdActividad = Convert.ToInt16(idActividad);
            // eventos
            guardar.Clicked += Modificar_Clicked;
            guardar.Text = "Guardar Cambios";
            cancelar.Clicked += Cancelar_Clicked1;
        }

        private async void Modificar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre.Text) || string.IsNullOrEmpty(tolerancia.Text))
                {
                    await DisplayAlert("Modificar Actividad", "Debe rellenar todos los campos.", "OK");
                    return;
                }
                object[,] variables = new object[,] {
                        { "idPlantillaActividad", IdPlantilla} ,{ "nombre", nombre.Text}, { "tolerancia", tolerancia.Text }, { "idActividad", IdActividad } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantillaActividad.asmx", "ModificarPlantillaActividad", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Modificar Plantilla", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Modificar Actividad", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
            
            //Navigation.PopAsync();
        }
        #endregion
        #region ================agregar actividad====================================
        private async void Agregar_Clicked(object sender, EventArgs e)
        {

            try
            {

                if (string.IsNullOrEmpty(nombre.Text) || string.IsNullOrEmpty(tolerancia.Text))
                {
                    await DisplayAlert("Agregar Actividad", "Debe rellenar todos los campos.", "OK");
                    return;
                }
                object[,] variables = new object[,] { { "nombre", nombre.Text }, { "tolerancia", tolerancia.Text }, { "idActividad", IdPlantilla } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantillaActividad.asmx", "IngresarPlantillaActividad", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Agregar Actividad", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Actividad", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }

        }
        #endregion

        // ===================// GuardarCambios // ===================//
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        private async void GuardarCambios_Clicked(object sender, EventArgs e)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        {
        }
        // ===================// Cancelar // =================== //
        private void Cancelar_Clicked1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        // ===================//
    }
}
