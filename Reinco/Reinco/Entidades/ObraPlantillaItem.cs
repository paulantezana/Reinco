using Reinco.Interfaces;
using Reinco.Interfaces.Supervision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class ObraPlantillaItem
    {


        public string nombre { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int idPropietarioObra { get; set; }
        public int idPlantillaObra { get; set; }
        public int idObra { get; set; }
        public int idPlantilla { get; set; }


        public ICommand PlantillaSupervision { get; private set; }
       //public ICommand AgregarPlantilla { get; private set; }



        public ObraPlantillaItem()
        {
            PlantillaSupervision = new Command(() =>
            {
                App.ListarObraPlantilla.Navigation.PushAsync(new ListarPlantillaSupervision(idPlantillaObra,idObra,idPlantilla));
            });
        }


    }
}
