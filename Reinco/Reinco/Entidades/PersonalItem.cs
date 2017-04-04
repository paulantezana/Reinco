using Reinco.Interfaces.Personal;
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
        public int idCargo { get; set; }
        public string cargo { get; set; }

        public ICommand editarUsuario { get; private set; }

        public PersonalItem()
        {
             editarUsuario = new Command(() =>
                {
                    App.ListarPersonal.Navigation.PushAsync(new AgregarPersonal
                        (this.idUsuario, this.dni, this.nombresApellidos, this.usuario, this.contrasena,
                        this.correo, this.celular, this.cip, this.idCargo,this.idCargo_Usuario));
                });
        }
    }
}
