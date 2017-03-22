using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public DialogService dialogService;
        public WebService webService;
        public LoginPage()
        {
            InitializeComponent();
            enviar.Clicked += Enviar_Clicked;
            dialogService = new DialogService();
            
        }
        
        private async void Enviar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usuario.Text))
            {
                await dialogService.MostrarMensaje("Iniciar Sessión", "Debes Ingresar Un Nombre de usuario");
                return;
            }
            if (string.IsNullOrEmpty(password.Text))
            {
                await dialogService.MostrarMensaje("Iniciar Sessión", "Debes Ingresar La Contraseña");
                return;
            }

            // var usuario = await webService.Get 

            //var client = new HttpClient();
            //client.BaseAddress = new Uri("http://192.168.1.37:8091");
            //var url = "/ServicioUsuario.asmx/MostrarUsuariosTable";
            //var response = await client.GetAsync(url);

            App.Current.MainPage = new MainPage();
        }
    }
}
