using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Gestores
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
        public string colorCargo { get; set; }
        public bool visible_asignarPlantilla { get; set; }
        public bool visible_asignarObra { get; set; }
        /*atributos generados por condicionales*/

        #region +---- Comandos ---+
        public ICommand asignarPlantilla { get; private set; }
        public ICommand asignarObra { get; private set; }
        #endregion

        public PersonalItem()
        {
            asignarPlantilla = new Command(() =>
            {
                //App.ListarObra.Navigation.PushAsync(new AsignarPlantilla(this.idUsuario, this.nombre));
            });

            asignarObra = new Command(() =>
            {
               // App.ListarObra.Navigation.PushAsync(new AgregarObra(this.idUsuario, this.codigo, this.nombre));
            });
        }
    }
}
