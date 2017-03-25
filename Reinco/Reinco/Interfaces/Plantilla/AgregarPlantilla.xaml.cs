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

        // ===================== Listar PLantillas ===================== //
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(codPlantilla.Text) || string.IsNullOrEmpty(nombrePlantilla.Text) )
            {
                await dialogService.MostrarMensaje("Agregar usuario", "debe rellenar todos los campos");
                return;
            }
            
                using (var cliente = new HttpClient())
                {
                    var result = await cliente.GetAsync("http://192.168.1.37/ServicioPlantilla.asmx/IngresarPlantilla?codigo=" + codPlantilla.Text + "&nombre=" + nombrePlantilla.Text);
                    if (result.IsSuccessStatusCode)
                    {
                        await App.Current.MainPage.DisplayAlert("Agregar Plantilla", "Plantilla agregada satisfactoriamente", "OK");
                        return;
                    }
                }
            
        }
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
