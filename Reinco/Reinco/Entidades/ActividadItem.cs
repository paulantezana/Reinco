using Reinco.Interfaces.Plantilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class ActividadItem
    {
        public int idActividad { get; set; }
        public string nombre { get; set; }
        public string tolerancia { get; set; }
        public byte enumera { get; set; }
        public int idPlantilla { get; set; }
        #region=============comandos===============
        public ICommand editarActividad { get; private set; }
        #endregion

        #region===============constructor(editar plantilla)====================
        public ActividadItem()
        {
            editarActividad = new Command(() =>
            {
                App.ListarActividad.Navigation.PushAsync(new AgregarActividad(this.idActividad, this.nombre, this.tolerancia, this.idPlantilla));
            });
        }

        #endregion
    }
}
