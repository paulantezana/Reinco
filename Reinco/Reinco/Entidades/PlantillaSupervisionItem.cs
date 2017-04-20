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
        public int idSupervision { get; set; }
        public string nombre { get; set; }
        public int numero { get; set; }
        public string fecha { get; set; }
        public int idObra { get; set; }
        public int idPlantilla { get; set; }
        public string partidaEvaluada { get; set; }
        public string nivel { get; set; }
        public string nombreAsistente { get; set; }
        public string nombreResponsable { get; set; }
        public ICommand Supervisar { get; private set; }
        public ICommand verActividades { get; private set; }
        public string colorSupervision { get;set; }
        public string nombreObra { get; set; }


        public PlantillaSupervisionItem()
        {
            Supervisar = new Command(() =>
            { 
                //App.ListarPlantillaSupervision.Navigation.PushAsync(new Supervisar());
            });
            verActividades = new Command(() =>
            {
                App.ListarPlantillaSupervision.Navigation.PushAsync(new Supervisar(idSupervision,nombreObra));
                //App.ListarPlantillaSupervision.Navigation.PushAsync(new Supervisar());
            });
        }
    }
}
