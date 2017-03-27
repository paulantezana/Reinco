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
        DialogService dialogService;

        // ===================== Constructor Para Crear Plantilla =================== //
        public AgregarPlantilla()
        {
            InitializeComponent();
            // servicios
            dialogService = new DialogService();

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
            if (string.IsNullOrEmpty(codPlantilla.Text) || string.IsNullOrEmpty(nombrePlantilla.Text) )
            {
                await dialogService.MostrarMensaje("Agregar plantilla", "Debe rellenar todos los campos.");
                return;
            }
            
                using (var cliente = new HttpClient())
                {
                    var result = await cliente.GetAsync("http://192.168.1.37:8080/ServicioPlantilla.asmx/IngresarPlantilla?codigo=" + codPlantilla.Text + "&nombre=" + nombrePlantilla.Text);
                    var json = await result.Content.ReadAsStringAsync();
                    //dynamic array = JsonConvert.DeserializeObject(json);
                string mensaje = Convert.ToString(json);
                    //si no existe el usuario o la contraseña es incorrecta, devuelve mensaje de error
                   
                if (result.IsSuccessStatusCode)
                    {
                        await App.Current.MainPage.DisplayAlert("Agregar Plantilla", mensaje, "OK");
                        return;
                    }
                }
            
        }
        #endregion
        // ===================== Constructor Para Actualizar O Cambiar Plantilla ===================== //
        public AgregarPlantilla(object idPlantilla)
        {
            InitializeComponent();
            guardar.Text = "Guardar Cambios";
            cancelar.Clicked += Cancelar_Clicked1;
        }

        private void Cancelar_Clicked1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        // ===================== END
    }
}
