
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
            try
            {
                if (string.IsNullOrEmpty(usuario.Text) || string.IsNullOrEmpty(password.Text))
                {
                    await dialogService.MostrarMensaje("Iniciar Sesión", "Los campos no deben estar vacios");
                    return;
                }
                var client = new HttpClient();
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "usuario", usuario.Text }, { "contrasenia", password.Text } };
                dynamic result = servicio.MetodoGet("ServicioUsuario.asmx", "Login", variables);
                //envia por metodo get los datos introducidos en los textbox
               // var result = await client.GetAsync("http://192.168.1.37:80/ServicioUsuario.asmx/Login?usuario=" + usuario.Text + "&contrasenia=" + password.Text);
                //si no existe el usuario, manda un mensaje de error
                /*if (result)
                {
                    await App.Current.MainPage.DisplayAlert("error al iniciar sesion", "ingrese nuevamente sus datos", "OK");
                    return;
                }*/
                //recoge los datos json y los almacena en la variable resultado
                //var resultado = await result.Content.ReadAsStringAsync();
                //si todo es correcto, muestra la pagina que el usuario debe ver
                //List<Usuario> items = JsonConvert.DeserializeObject<List<Usuario>>(resultado);
                //DataTable dtUsuario = new DataTable();
               // dynamic array = JsonConvert.DeserializeObject(resultado);

                App.Current.MainPage = new MainPage();
            }
            catch
            {

            }
          
        }
    }
}
