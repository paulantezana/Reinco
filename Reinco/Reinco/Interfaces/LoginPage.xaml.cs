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
        public DialogService dialogService;
        public ObservableCollection<ObraItem> obraitem { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            enviar.Clicked += Enviar_Clicked;
            dialogService = new DialogService();

            obraitem = new ObservableCollection<ObraItem>();
            LoadObras();
            title.ItemsSource = obraitem;
        }

        private void LoadObras()
        {
            for (int i = 0; i < 30; i++)
            {
                obraitem.Add(new ObraItem
                {
                    titulo = "Nombre De La Obra",
                    responsable = "nombre del responsable",
                    platilla = "PLANTILLAS"
                });
            }
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


            //var client = new HttpClient();
            //// client.BaseAddress = new Uri("http://192.168.1.37:8091");
            //// var url = "/ServicioUsuario.asmx/MostrarUsuariosTable";
            //var response = await client.GetAsync("http://192.168.1.37/ServicioUsuario.asmx/LogueoUsuarioAdmin?usuario=admin&contrasenia=admin");
            //string result = await response.Content.ReadAsStringAsync();
            //dynamic json = JsonConvert.DeserializeObject(result);

            //if(json[0].cargo == "Administrador")
            //{
            //    await dialogService.MostrarMensaje("Ok", "Eres el Administrador");
            //}

            App.Current.MainPage = new MainPage();
        }
    }
}
