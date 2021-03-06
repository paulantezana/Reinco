using Reinco.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Reinco.Recursos;
using System.Text.RegularExpressions;
using Reinco.Interfaces.Plantilla;
using Reinco.Interfaces.Supervision;
using Reinco.Interfaces.Obra;
using Reinco.Interfaces.Personal;
using Reinco.Interfaces.Propietario;
using System.Collections.ObjectModel;
using Reinco.Entidades;

namespace Reinco
{
    public partial class App : Application
    {
        #region +---- Atributos ----+
        public static string opcional = "(Opcional)";
        public static string ip;
        public static string puerto = "80";
        public static string cuenta = "reinco_pruebas_code";
        public static string cargo = "";
        public static string correo = "";
        public static string directorio = "Reinco";
        public static int ultimoNroSupervision = 0;
        public VentanaMensaje mensaje;
        public static int nroElementos = 10;
        public static int idSupervisionActividadE;
        public static int terminoCargadoSupervisiones = 0;
        public static int idUsuarioAsistente = 0;
        public static string nombreUsuarioAsistente = "";
        public static ObservableCollection<FotosxActividadItem> FotosxActividadItemsE { get; set; }
        #endregion

        #region +---- Propiedades ----+

        public static ListarObra ListarObra { get; internal set; } // Página listar Obra
        public static ListarPlantilla ListarPlantilla { get; internal set; } // Página listar Plantilla
        public static ListarActividad ListarActividad { get; internal set; } // Página listar actividades
        public static ListarPropietario ListarPropietarios { get; internal set; } // Página listar propietarios
        public static ListarObraPlantilla ListarObraPlantilla { get; internal set; }
        public static ListarPlantillaSupervision ListarPlantillaSupervision { get; internal set; }
        //public static ListarObrasAdmin ListarObrasAdmin { get; internal set; }
        public static MainPage Navigator { get; internal set; }
        public static ListarPersonal ListarPersonal { get; internal set; }
        public static ListarObraxCargo ListarObraxCargo { get; internal set; }
        public static Supervisar Supervisar { get; internal set; }

        #endregion

        #region +---- Constrcutor -----+
        public App()
        {
            //recupero por única vez cuando entro a la aplicación el IP de servidor
            mensaje = new VentanaMensaje();
            //ObtenerIpAsync();
            //ip = "192.168.1.111";
           // ip = "192.168.1.36";
           ip = "190.117.145.7";
            //ip = "190.42.122.110";

            InitializeComponent();
            MainPage = new LoginPage();
           //MainPage = new Cargos();
            Current.Properties["direccionApp"] = "";
        }

        #endregion

        #region +---- Comunicacion ----+
        public async void ObtenerIpAsync()
        {
            await LeerUrlAsync("http://codeperu.com/ip/ip.txt");//http://...
        }
        public async Task LeerUrlAsync(string url)
        {
            try
            {
                //await Task.Delay(40000);//borrar
                var cliente = new HttpClient();
                var message = await cliente.GetAsync(url);
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    ip = await message.Content.ReadAsStringAsync();
                    //await mensaje.MostrarMensaje("Información", ip);//borrar
                    if (!Regex.IsMatch(ip, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$"))//validar con lenguajes regulares 192.123.12.32
                        await mensaje.MostrarMensaje("Error", "Error: El URL está respondiendo, sin embargo el servidor no está actualizando su información");
                }
                else
                {
                    await mensaje.MostrarMensaje("Error", message.ToString());
                }
            }
            catch (Exception e)
            {
                await mensaje.MostrarMensaje("Error", e.ToString());
            }
        } 
        #endregion

        protected override void OnStart()
        {
            // Handle when your app starts
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
