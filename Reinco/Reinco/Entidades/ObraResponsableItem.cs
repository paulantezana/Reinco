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
        public string idObra { get; set; }
        public string nombre { get; set; }



        #region + ---------- Comandos ---------- +
        public ICommand PlantillaObra { get; private set; }
        #endregion



        #region + ---------- Constructor ---------- +
        public ObraResponsableItem()
        {
            PlantillaObra = new Command(() =>
            {
                App.ListarObraResponsable.Navigation.PushAsync(new ListarObraPlantilla(idObra));
            });
        } 
        #endregion



    }
}
