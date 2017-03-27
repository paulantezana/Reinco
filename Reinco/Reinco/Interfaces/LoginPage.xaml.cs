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
using System.Text.RegularExpressions;

namespace Reinco.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage
    {
        //login page
        public VentanaMensaje mensaje;
        public LoginPage()
        {
            InitializeComponent();
            mensaje = new VentanaMensaje();
            enviar.Clicked += Enviar_Clicked;
            /*enviar.IsEnabled = false;
            VerificarIP();*/    
        }
        /*public Task VerificarIP()
        {
            return Task.Run(() =>
            {
                bool ipActivo = false;
                while (!ipActivo)
                {
                    if (Regex.IsMatch(App.ip, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$"))
                        ipActivo = true;
                }
                enviar.IsEnabled = true;
            });
        }*/
        private async void Enviar_Clicked(object sender, EventArgs e)
        {
            try
            {
                enviar.IsEnabled = false;
                if (string.IsNullOrEmpty(usuario.Text) || string.IsNullOrEmpty(password.Text))
                {
                    await mensaje.MostrarMensaje("Iniciar Sesión", "Ninguno de los campos debe estar vacio");
                    enviar.IsEnabled = true;
                    return;
                }
                var client = new HttpClient();
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "usuario", usuario.Text }, { "contrasenia", password.Text } };
                dynamic result = await servicio.MetodoGet("ServicioUsuario.asmx", "Login", variables);
                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        await mensaje.MostrarMensaje("Iniciar Sesión", "Usuario o contraseña incorrecta!");
                        enviar.IsEnabled = true;
                    }
                    else
                    { 
                        // ---------- Almacenando Datos Usuario En Local -------------------//
                        Application.Current.Properties["idUsuario"] = result[0].idUsuario;
                        Application.Current.Properties["nombreUsuario"] = result[0].nombres;
                        Application.Current.Properties["apellidoUsuario"] = result[0].apellidos;
                        Application.Current.Properties["cargoUsuario"] = result[0].cargo;
                        App.Current.MainPage = new MainPage();
                    }
                }
                else
                {
                    await mensaje.MostrarMensaje("Iniciar Sesión", "Error de respuesta del servicio, Contáctese con el administrador");
                    enviar.IsEnabled = true;
                }
                //List<Usuario> items = JsonConvert.DeserializeObject<List<Usuario>>(resultado);
                //DataTable dtUsuario = new DataTable();
                // dynamic array = JsonConvert.DeserializeObject(resultado);
            }
            catch(Exception ex)
            {
                await mensaje.MostrarMensaje("Iniciar Sesión", "Error en el dispositivo o URL incorrecto: "+ex.ToString());
                enviar.IsEnabled = true;
            }
        }
    }
}
