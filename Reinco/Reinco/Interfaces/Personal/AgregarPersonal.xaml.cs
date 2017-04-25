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
                    guardar.IsEnabled = true;
                    return;
                }
                int contadorCargos = 0;
                int idSupervisor = 0;
                int idResponsable = 0;
                int idGerente = 0;
                #region=====cargo=====
                if (supervisor.IsToggled == true) {
                    idSupervisor = 3;//supervisor
                    contadorCargos++;
                }

                if (responsable.IsToggled == true) {
                    idResponsable = 2;//responsable
                    contadorCargos++;
                }

                if (gerente.IsToggled == true) {
                    idGerente = 1;//admin
                    contadorCargos++;
                }
                int[] idCargo = new int[contadorCargos];
                bool revisado1 = false;
                bool revisado2 = false;
                bool revisado3 = false;
                for (int i = 0; i < contadorCargos; i++)
                {

                    if (supervisor.IsToggled == true && revisado3 == false)
                    {
                        idCargo[i] = 3;
                        revisado3 = true;
                        continue;
                    }

                    if (responsable.IsToggled == true && revisado2 == false)
                    {
                        revisado2 = true;
                        idCargo[i] = 2;
                        continue;
                    }
                    if (gerente.IsToggled == true&&revisado1==false)
                    {
                        revisado1 = true;
                        idCargo[i] = 1;
                        continue;
                    }
                        
                }
                
                    #endregion
                if (contra.Text == confirmarContra.Text)
                {
                    object[,] variables = new object[contadorCargos + 7, 2];
                    variables[0, 0] = "dni";
                    variables[0, 1] = dni.Text;
                    variables[1, 0] = "nombresApellidos";
                    variables[1, 1] = nombresApellidos.Text;
                    variables[2, 0] = "usuario";
                    variables[2, 1] = usuario.Text;
                    variables[3, 0] = "contrasenia";
                    variables[3, 1] = contra.Text;
                    variables[4, 0] = "correo";
                    variables[4, 1] = email.Text;
                    variables[5, 0] = "cip";
                    variables[5, 1] = cip.Text == null ? "" : cip.Text;
                    string identificador = "idCargo";
                    int j = 6;//numoero de objetos que estan en la variable
                    int k = 0;//index del arreglo de cargos
                    for (int i = 6; i < contadorCargos + 6; i++)
                    {
                        int numero = Convert.ToInt16(idCargo[k]);
                        variables[i, 0] = identificador;
                        variables[i, 1] = numero;
                        j++;
                        k++;
                    }
                    // Desde aqui logica para enviar al web service
                    variables[j, 0] = "celular";
                    variables[j, 1] = celular.Text == null ? "" : celular.Text;
                    dynamic result = await Servicio.MetodoGetString("ServicioUsuario.asmx", "AgregarUsuario", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        guardar.IsEnabled = false;
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
                await App.Current.MainPage.DisplayAlert("Agregar Usuario", "Error con el servidor o dispositivo"+ex.Message, "OK");
                return;
            }
        }
        public AgregarPersonal(int idUsuario,string dni, string nombresApellidos, string usuario,string contra,string correo,
            string celular,string cip,int idCargo,int idCargoUsuario)//-----Modificar usuario
        {
            InitializeComponent();
            guardar.Text = "Guardar Cambios";
            lblCelular.Text = "Celular " + App.opcional;
            lblCIP.Text = "CIP " + App.opcional;
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
                    await App.Current.MainPage.DisplayAlert("Modificar Usuario","Rellene todos los campos", "OK");
                    guardar.IsEnabled = true;
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
                        guardar.IsEnabled = false;
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
                //await mensaje.MostrarMensaje("Agregar Usuario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
                await App.Current.MainPage.DisplayAlert("Modificar Usuario",ex.Message, "OK");
                return;
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
