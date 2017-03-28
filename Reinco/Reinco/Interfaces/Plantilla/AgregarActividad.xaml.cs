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
            if (string.IsNullOrEmpty(nombre.Text) || string.IsNullOrEmpty(tolerancia.Text))
            {
                await DisplayAlert("Modificar Actividad", "Debe rellenar todos los campos.", "OK");
                return;
            }
            using (var cliente = new HttpClient())
            {
                var result = await cliente.GetAsync("http://192.168.1.37:8080/ServicioPlantillaActividad.asmx/ModificarPlantillaActividad?idPlantillaActividad="
                    + IdPlantilla + "&nombre=" + nombre.Text + "&tolerancia=" + tolerancia.Text + "&idActividad=" + IdActividad);
                var json = await result.Content.ReadAsStringAsync();
                string mensaje = Convert.ToString(json);
                //comentario
                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Modificar Actividad", mensaje, "OK");
                    return;
                }
            }
            //Navigation.PopAsync();
        }
        #endregion
        #region ================agregar actividad====================================
        private async void Agregar_Clicked(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(nombre.Text) || string.IsNullOrEmpty(tolerancia.Text))
            {
                await DisplayAlert("Agregar Actividad", "Debe rellenar todos los campos.", "OK");
                return;
            }

            using (var cliente = new HttpClient())
            {
                var result = await cliente.GetAsync("http://192.168.1.37:8080/ServicioPlantillaActividad.asmx/IngresarPlantillaActividad?nombre="
                    + nombre.Text + "&tolerancia=" + tolerancia.Text + "&idActividad=" + IdPlantilla);
                var json = await result.Content.ReadAsStringAsync();
                string mensaje = Convert.ToString(json);

                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Agregar Actividad", mensaje, "OK");
                    return;
                }
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
