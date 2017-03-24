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
        public AgregarPlantilla()
        {
            InitializeComponent();
            guardar.Clicked += Guardar_Clicked;
            dialogService = new DialogService();
        }
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
    }
}
