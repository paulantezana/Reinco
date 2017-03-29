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

namespace Reinco
{
    public partial class App : Application
    {
        public static string ip;
        public static string puerto = "8080";
        public VentanaMensaje mensaje;

        public App()
        {
            //recupero por única vez cuando entro a la aplicación el IP de servidor
            mensaje = new VentanaMensaje();
            //ObtenerIpAsync();
            //ip = "192.168.1.111";
            ip = "192.168.1.37";
            InitializeComponent();
            MainPage = new LoginPage();
        }
        public async void ObtenerIpAsync()
        {
            await LeerUrlAsync("ip/ip.txt");//http://...
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
                    await mensaje.MostrarMensaje("Información", ip);//borrar
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
