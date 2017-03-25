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
        DialogService dialogService;
        private object idPropietario;

        // ===================== Constructor Para Crear Propietario ===================== //
        public AgregarPropietario()
        {
            InitializeComponent();
            guardar.Clicked += Guardar_Clicked;
            dialogService = new DialogService();
            // eventos
            cancelar.Clicked += Cancelar_Clicked;
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
        // ===================== Constructor Para Actualizar O Cambiar Propietario ===================== //
        public AgregarPropietario(object idPropietario)
        {
            InitializeComponent();
            this.idPropietario = idPropietario;
            guardar.Text = "Guardar Cambios";
            cancelar.Clicked += Cancelar_Clicked;
        }

        // Cacelar ============
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
