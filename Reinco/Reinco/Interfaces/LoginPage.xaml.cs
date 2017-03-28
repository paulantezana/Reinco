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
using System.Diagnostics;

namespace Reinco.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage
    {
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


        #region * ================================ Iniciando Sesión ================================ *
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
                        // ---------- Almacenando Los Datos Del Usuario  En Local -------------------//
                        Application.Current.Properties["idUsuario"] = result[0].idUsuario;
                        Application.Current.Properties["nombreUsuario"] = result[0].nombres;
                        Application.Current.Properties["apellidoUsuario"] = result[0].apellidos;
                        Application.Current.Properties["cargoUsuario"] = result[0].cargo;
                        // await Application.Current.SavePropertiesAsync(); // Active esta opcion si dese guardar en el movil permanentemente

                        App.Current.MainPage = new MainPage(); // Navegando a la página Main page ( Páina Principal que conecta los de mas páginas)
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
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Iniciar Sesión", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
                enviar.IsEnabled = true;
            }
        } 
        #endregion


        #region // ============================== Recuperar Contraseña ============================== //
        private async void recuperarContra(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Recuperar Por:", "Cancel", null, "Email", "Contactarse Con El Administrador");
            await DisplayAlert("Respuesta Temporal", action + " Lo sentimos esta funcionalidad aun no esta disponible", "Aceptar");
        } 
        #endregion


    }
}
