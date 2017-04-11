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
        int IdCargoUsuario;
        int IdCargo;
        VentanaMensaje dialogService;


        #region +---- Eventos ----+
        //new public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        public AgregarPersonal()
        {
            InitializeComponent();
            #region================labels===================
            lblCelular.Text = "Celular " + App.opcional;
            lblCIP.Text = "CIP " + App.opcional;
            #endregion
            guardar.Clicked += Guardar_Clicked;
            dialogService = new VentanaMensaje();
            cancelar.Clicked += Cancelar_Clicked;
        }
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            guardar.IsEnabled = false;
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
                        { "dni", dni.Text }, { "nombresApellidos", nombresApellidos.Text },{ "usuario", usuario.Text },
                       { "contrasenia", contra.Text },  { "correo", email.Text },{ "cip", cip.Text==null?"":cip.Text },
                       { "idCargo", enviarCargo }, { "celular",celular.Text==null?"":celular.Text} };
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
        public AgregarPersonal(int idUsuario,string dni, string nombresApellidos,string usuario,string contra,string correo,
            string celular,string cip,int idCargo,int idCargoUsuario)
        {
            InitializeComponent();
            guardar.Text = "Guardar Cambios";
            this.Title = usuario;
            IdUsuario =idUsuario;
            IdCargoUsuario = idCargoUsuario;
            this.dni.Text = dni;
            this.nombresApellidos.Text = nombresApellidos;
            this.usuario.Text = usuario;
            this.email.Text = correo;
            this.celular.Text = celular;
            this.cip.Text = cip;
            if (idCargo == 1)
                gerente.IsToggled=true;
            if (idCargo == 2)
                responsable.IsToggled= true;
            if (idCargo == 3)
                supervisor.IsToggled = true;
            cancelar.Clicked += Cancelar_Clicked;
            guardar.Clicked += ModificarUsuario_Clicked1;
        }
        #region==============Modificar Usuario======================================
        private async void ModificarUsuario_Clicked1(object sender, EventArgs e)
        {
            guardar.IsEnabled = false;
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
                    if (supervisor.IsToggled == true)
                        IdCargo = 3;//supervisor
                    if (responsable.IsToggled == true)
                        IdCargo = 2;//responsable
                    if (gerente.IsToggled == true)
                        IdCargo = 1;//admin
                    object[,] variables = new object[,] {
                        { "idUsuario", IdUsuario } ,{ "dni", dni.Text }, { "nombresApellidos", nombresApellidos.Text },
                        { "usuario", usuario.Text }, { "contrasenia", contra.Text }, { "correo", email.Text },{ "cip", cip.Text },
                        { "celular", celular.Text },{ "idCargo",IdCargo  },{ "idCargoUsuario",IdCargoUsuario  }};
                    dynamic result = await Servicio.MetodoGetString("ServicioUsuario.asmx", "ModificarUsuario", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Modificar Usuario", Mensaje, "OK");
                        App.ListarPersonal.Personaltems.Clear();
                        App.ListarPersonal.CargarPersonalItem();
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
        #endregion
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        #region=============longitud maxima dni=============0
        public void Limitante(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 8)
                dni.Text = dni.Text.Remove(8);
      }  
        #endregion
    }

}
