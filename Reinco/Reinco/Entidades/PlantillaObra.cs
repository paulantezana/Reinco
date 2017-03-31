using Reinco.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class PlantillaObra
    {
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }

        public ICommand ejecutarTap { get; private set; }
        public ICommand nuevaEje { get; private set; }
        public PlantillaObra()
        {
            ejecutarTap = new Command(() =>
            {
                App.Current.MainPage.DisplayAlert("OK", " Me ejecute == ejecutarTap " + this.codigo + this.descripcion, "Aceptar");
            });
            nuevaEje = new Command(() =>
            {
                App.Current.MainPage.DisplayAlert("OK", " Me ejecute == nuevaEje "+ this.codigo + this.descripcion, "Aceptar");
            });
        }

    }
}
