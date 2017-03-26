using Reinco.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Reinco
{
    public partial class App : Application
    {
        public static string ip;
        public static string puerto = "8080";
        public App()
        {
            //recupero por única vez cuando entro a la aplicación el IP de servidor
            string ip = ObtenerIP("http://www.codeperu.com/ip/ip.txt").ToString();
            //ip = "192.168.1.111";
            InitializeComponent();
            MainPage = new LoginPage();
        }
        public static async Task<string> ObtenerIP(string url)
        {
            try
            {
                string contenido = "";
                var httpClient = new HttpClient();
                HttpResponseMessage message = (HttpResponseMessage)await httpClient.GetAsync(url);
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    contenido = await message.Content.ReadAsStringAsync();
                }
                return contenido;
            }
            catch (Exception e)
            {
                return e.ToString();
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
