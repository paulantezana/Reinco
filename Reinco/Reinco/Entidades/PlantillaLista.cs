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
    public class PlantillaLista
    {
        public int idPlantilla { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }

        #region=============comandos===============
        public ICommand editarPlantilla { get; private set; }
        #endregion

        #region===============constructor(editar plantilla)====================
        public PlantillaLista()
        {
            editarPlantilla = new Command(() =>
            {
                App.ListarPlantilla.Navigation.PushAsync(new AgregarPlantilla(this.idPlantilla, this.codigo, this.nombre));
            });
        }
        
        #endregion
    }
}
