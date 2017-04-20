using Reinco.Recursos;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using System.Diagnostics;

namespace Reinco.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        #region +---- Atributos ----+
        public VentanaMensaje mensaje;
        private bool isRunning;
        //string CargoUsuario;
        #endregion


        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        #region +---- Constructor ----+
        public LoginPage()
        {
            InitializeComponent();
            mensaje = new VentanaMensaje();
            enviar.Clicked += Enviar_Clicked;

            // TEST
            this.BindingContext = this; // linea que define el contexto del XAML
            // TEST
            /*enviar.IsEnabled = false;
            VerificarIP();*/
        } 
        #endregion


        #region +---- Propiedades ----+
        public bool IsRunning {
            set
            {
                if(isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }

        #endregion
        

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
            cambiarEstado(false);
            try
            {
                
                if (string.IsNullOrEmpty(usuario.Text) || string.IsNullOrEmpty(password.Text))
                {
                    cambiarEstado(true);
                    await mensaje.MostrarMensaje("Iniciar Sesión", "Ninguno de los campos debe estar vacio");
                    return;
                }
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "usuario", usuario.Text }, { "contrasenia", password.Text } };
                dynamic result = await servicio.MetodoGet("ServicioUsuario.asmx", "Login", variables);
                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        cambiarEstado(true);
                        await mensaje.MostrarMensaje("Iniciar Sesión", "Usuario o contraseña incorrecta!");
                    }
                    else
                    {
                        foreach (var usuario in result)
                        {
                            if(usuario.cargo == "Gerente")
                            {
                                Application.Current.Properties["idUsuario"] = usuario.idUsuario;
                                Application.Current.Properties["nombresApellidos"] = usuario.nombresApellidos;
                                Application.Current.Properties["cargoUsuario"] = usuario.cargo;
                                App.cargo = usuario.cargo;
                                break;
                            }
                            else if(usuario.cargo == "Asistente")
                            {
                                Application.Current.Properties["idUsuario"] = usuario.idUsuario;
                                Application.Current.Properties["nombresApellidos"] = usuario.nombresApellidos;
                                Application.Current.Properties["cargoUsuario"] = usuario.cargo;
                                App.cargo = usuario.cargo;
                                break;
                            }else if(usuario.cargo == "Responsable")
                            {
                                Application.Current.Properties["idUsuario"] = usuario.idUsuario;
                                Application.Current.Properties["nombresApellidos"] = usuario.nombresApellidos;
                                Application.Current.Properties["cargoUsuario"] = usuario.cargo;
                                App.cargo = usuario.cargo;
                            }
                        }
                        //Application.Current.Properties["cargoUsuario2"] = result
                        App.Current.MainPage = new MainPage(); // Navegacion a la pagina usuario
                        
                    }
                }
                else
                {
                    cambiarEstado(true);
                    await mensaje.MostrarMensaje("Iniciar Sesión", "Error de respuesta del servicio, Contáctese con el administrador");
                    return;
                }
            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await mensaje.MostrarMensaje("Iniciar Sesión", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
                return;
            }
            finally
            {
                cambiarEstado(true);
                enviar.IsEnabled = true;
            }
        } 
        #endregion


        #region // ============================== Recuperar Contraseña ============================== //
        //private async void recuperarContra(object sender, EventArgs e)
        //{
        //    var action = await DisplayActionSheet("Recuperar Por:", "Cancel", null, "Email", "Contactarse Con El Administrador");
        //    await DisplayAlert("Respuesta Temporal", action + " Lo sentimos esta funcionalidad aun no esta disponible", "Aceptar");
        //} 
        #endregion

        public void  cambiarEstado(bool estado)
        {
            usuario.IsEnabled = estado;
            password.IsEnabled = estado;
            enviar.IsEnabled = estado;
            if (estado == true) {
                IsRunning = false;
            }
            else
            {
                IsRunning = true;
            }
            
        }

    }
}
