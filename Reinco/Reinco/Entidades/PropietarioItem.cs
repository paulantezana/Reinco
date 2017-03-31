using Reinco.Interfaces.Propietario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class PropietarioItem
    {
        public int idPropietario { get; set; }
        public string nombre { get; set; }
        public string fotoPerfil { get; set; }
        #region=======comand================
        public ICommand editarPropietario { get; private set; }
        #endregion
        public PropietarioItem()
        {
            editarPropietario = new Command(() =>
            {
                App.ListarPropietarios.Navigation.PushAsync(new AgregarPropietario(this.idPropietario, this.nombre));
            });
        }
    }
}
