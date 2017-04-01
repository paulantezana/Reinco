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
    public class PlantillaItem
    {
        public int idPlantilla { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }

        #region=============comandos===============
        public ICommand EditarPlantilla { get; private set; }
        public ICommand Actividad { get; private set; }
        #endregion

        #region===============constructor(editar plantilla)====================
        public PlantillaItem()
        {
            EditarPlantilla = new Command(() =>
            {
                App.ListarPlantilla.Navigation.PushAsync(new AgregarPlantilla(this.idPlantilla, this.codigo, this.nombre));
            });
            Actividad = new Command(() =>
            {
                App.ListarPlantilla.Navigation.PushAsync(new ListarActividad(this.idPlantilla,this.nombre));
            });
        }
        
        #endregion
    }
}
