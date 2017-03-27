using Newtonsoft.Json;
using Reinco.Gestores;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage
    {
        //login page
        public DialogService dialogService;
        public LoginPage()
        {
            InitializeComponent();
            enviar.Clicked += Enviar_Clicked;
            dialogService = new DialogService();
        }
        
        private async void Enviar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usuario.Text) || string.IsNullOrEmpty(password.Text))
            {
                await dialogService.MostrarMensaje("Iniciar Sessión", "Los campos no deben estar vacios.");
                return;
            }
            var client = new HttpClient();
            //envia por metodo get los datos introducidos en los textbox
            var result = await client.GetAsync("http://192.168.1.37:8080/ServicioUsuario.asmx/Login?usuario=" +
                usuario.Text + "&contrasenia=" + password.Text);
            //si surge algun error con el estado del servicio, devuelve un error
            if (!result.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Error al iniciar sesión",
                    "Problemas con la conexión, contáctese con el administrador.", "OK");
                return;
            }
            //recoge los datos json y los almacena en la variable resultado
            var resultado = await result.Content.ReadAsStringAsync();
            dynamic array = JsonConvert.DeserializeObject(resultado);
            //si no existe el usuario o la contraseña es incorrecta, devuelve mensaje de error
            if (array.Count == 0)
            {
                await App.Current.MainPage.DisplayAlert("Error al iniciar sesión",
                    "Usuario o clave incorrectos.", "OK");
                return;
            }
            App.Current.MainPage = new MainPage();

        }
    }
}
