using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Personal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarPersonal : ContentPage, INotifyPropertyChanged
    {
        WebService Servicio = new WebService();
        string Mensaje;
        public VentanaMensaje mensaje;
        int IdUsuario;
        VentanaMensaje dialogService;


        #region +---- Eventos ----+
        //new public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        public AgregarPersonal()
        {
            InitializeComponent();
            guardar.Clicked += Guardar_Clicked;
            dialogService = new VentanaMensaje();
            cancelar.Clicked += Cancelar_Clicked;
        }
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            int enviarCargo=0;
            try
            {
                if (string.IsNullOrEmpty(dni.Text) || string.IsNullOrEmpty(nombresApellidos.Text) || 
                    string.IsNullOrEmpty(usuario.Text) || string.IsNullOrEmpty(contra.Text) || string.IsNullOrEmpty(confirmarContra.Text)
                    || string.IsNullOrEmpty(email.Text))
                {
                    await dialogService.MostrarMensaje("Agregar Usuario", "debe rellenar todos los campos");
                    return;
                }
                #region=====cargo=====
                if (supervisor.IsToggled == true)
                    enviarCargo = 3;//supervisor
                if (responsable.IsToggled == true)
                    enviarCargo = 2;//responsable
                if (gerente.IsToggled == true)
                    enviarCargo = 1;//admin
                #endregion
                if (contra.Text == confirmarContra.Text)
                {
                    object[,] variables = new object[,] {
                        { "dni", dni.Text }, { "nombresApellidos", nombresApellidos.Text },
                        { "usuario", usuario.Text }, { "contrasenia", contra.Text }, { "correo", email.Text },{ "cip", cip.Text },{ "idCargo", enviarCargo }, { "celular",celular.Text} };
                    dynamic result = await Servicio.MetodoGetString("ServicioUsuario.asmx", "AgregarUsuario", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Agregar Usuario", Mensaje, "OK");
                        App.ListarPersonal.IsRefreshingPersonal = true;
                        App.ListarPersonal.Personaltems.Clear();
                        App.ListarPersonal.CargarPersonalItem();
                        App.ListarPersonal.IsRefreshingPersonal = false;
                        await Navigation.PopAsync();
                        return;
                    }

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Agregar Usuario", "Las contraseñas no coinciden, inténtelo nuevamente.", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Usuario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        public AgregarPersonal(object idUsuario)
        {
            InitializeComponent();
            guardar.Text = "Guardar Cambios";
            IdUsuario = Convert.ToInt16(idUsuario);
            cancelar.Clicked += Cancelar_Clicked;
            guardar.Clicked += ModificarUsuario_Clicked1;
        }
        #region==============Modificar Usuario======================================
        private async void ModificarUsuario_Clicked1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(dni.Text) || string.IsNullOrEmpty(nombresApellidos.Text)  ||
                    string.IsNullOrEmpty(usuario.Text) || string.IsNullOrEmpty(contra.Text) || string.IsNullOrEmpty(confirmarContra.Text)
                    || string.IsNullOrEmpty(email.Text))
                {
                    await dialogService.MostrarMensaje("Modificar Usuario", "debe rellenar todos los campos");
                    return;
                }
                if (contra.Text == confirmarContra.Text)
                {
                    object[,] variables = new object[,] {
                        { "idUsuario", IdUsuario } ,{ "dni", dni.Text }, { "nombresApellidos", nombresApellidos.Text },
                        { "usuario", usuario.Text }, { "contrasenia", contra.Text }, { "correo", email.Text },{ "cip", cip.Text }};
                    dynamic result = await Servicio.MetodoGetString("ServicioUsuario.asmx", "ModificarUsuario", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Modificar Usuario", Mensaje, "OK");
                        return;
                    }

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Agregar Usuario", "Las contraseñas no coinciden, inténtelo nuevamente.", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Usuario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        #endregion
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }

}
