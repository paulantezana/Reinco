using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Reinco.Interfaces.Supervision;

namespace Reinco.Entidades
{
    public class PlantillaSupervisionItem
    {

        public string nombre { get; set; }



        public ICommand Supervisar { get; private set; }



        public PlantillaSupervisionItem()
        {
            Supervisar = new Command(() =>
            {
                
                App.ListarPlantillaSupervision.Navigation.PushAsync(new Supervisar());
            });
        }
        

    }
}
