using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Propietario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarPropietario : ContentPage
    {
        VentanaMensaje dialogService;
        public AgregarPropietario()
        {
            InitializeComponent();
            guardar.Clicked += Guardar_Clicked;
            dialogService = new VentanaMensaje();
            
        }
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nombrePropietario.Text) )
            {
                await dialogService.MostrarMensaje("Agregar propietario", "debe rellenar todos los campos");
                return;
            }

            using (var cliente = new HttpClient())
            {
                var result = await cliente.GetAsync("http://192.168.1.37/ServicioPropietario.asmx/IngresarPropietario?propietario=" + nombrePropietario.Text );
                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Agregar Propietario", "Propietario agregado satisfactoriamente", "OK");
                    return;
                }
            }

        }
    }
}
