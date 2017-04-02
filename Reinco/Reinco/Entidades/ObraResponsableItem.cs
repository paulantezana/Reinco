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
    public class ObraResponsableItem
    {


        public int idResponsable { get; set; }
        public int idObra { get; set; }
        public string nombre { get; set; }
        public string colorObra { get; set; }
        public string codigo { get; set; }
        public int idPlantillaObra { get; set; }
        public int idPropietarioObra { get; set; }



    #region + ---------- Comandos ---------- +
    public ICommand PlantillaObra { get; private set; }
        #endregion



        #region + ---------- Constructor ---------- +
        public ObraResponsableItem()
        {
            PlantillaObra = new Command(() =>
            {
                //App.ListarObras.Navigation.PushAsync(new AsignarPlantilla(idPropietarioObra));
                App.ListarObras.Navigation.PushAsync(new ListarObraPlantilla(idPropietarioObra,idObra));
            });
        } 
        #endregion



    }
}
