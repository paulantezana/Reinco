using GalaSoft.MvvmLight.Command;
using Reinco.Interfaces.Obra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Gestores
{
    public class ObraItem
    {
        public int idObra { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string responsable { get; set; }
        public string plantilla { get; set; }
        public string colorObra { get; set; }




        public ICommand asignarPlantilla { get; private set; }
        public ICommand editarObra { get; private set; }
        public ICommand eliminarObra { get; private set; }
        

        public ObraItem()
        {
            asignarPlantilla = new Command(() =>
            {
                App.ListarObra.Navigation.PushAsync(new AsignarPlantilla(this.idObra, this.nombre));
            });

            editarObra = new Command(() =>
            {
                App.ListarObra.Navigation.PushAsync(new AgregarObra(this.idObra, this.codigo, this.nombre));
            });

            eliminarObra = new Command(() =>
            {

            });
        }
        

    }
}
