using Reinco.Interfaces.Personal;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class PersonalItem
    {
        WebService Servicio = new WebService();

        public string fotoPerfil { get; set; }
        public int idUsuario { get; set; }
        public string dni { get; set; }
        public string nombresApellidos { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
        public string cip { get; set; }
        public int idCargo_Usuario { get; set; }
        public int idCargo1 { get; set; }
        public int idCargo2 { get; set; }
        public int idCargo3 { get; set; }

        public string cargo { get; set; }

        public ICommand editarUsuario { get; private set; }
        public ICommand Eliminar { get; private set; }

        public PersonalItem()
        {
             editarUsuario = new Command(() =>
            {
                App.ListarPersonal.Navigation.PushAsync(new AgregarPersonal
                    (this.idUsuario, this.dni, this.nombresApellidos, this.usuario, this.contrasena,
                    this.correo, this.celular, this.cip, this.idCargo1, this.idCargo2, this.idCargo3, this.idCargo_Usuario));
            });
            Eliminar = new Command(() =>
            {
                eliminar();
            });
        }

        #region======================= Eliminar Personal ====================================
        public async void eliminar()
        {
            try
            {
                bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Desea eliminar a: " + this.nombresApellidos + " ? ", "Aceptar", "Cancelar");
                if (!respuesta) return;

                object[,] variables = new object[,] { { "idUsuario", this.idUsuario } };

                dynamic result = await Servicio.MetodoGetString("ServicioUsuario.asmx", "EliminarUsuario", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Usuario", Mensaje, "Aceptar");
                    App.ListarPersonal.Personaltems.Clear();
                    App.ListarPersonal.CargarPersonalItem();
                    return;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Eliminar Usuario", "Error en el dispositivo o URL incorrecto: " + ex.Message,"Aceptar");
            }
        }
        #endregion

    }
}
