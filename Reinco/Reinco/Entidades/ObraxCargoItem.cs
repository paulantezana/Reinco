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
    public class ObraxCargoItem
    {
        public int idPropietario_Obra { get; set; }
        public int idObra { get; set; }
        public int idPropietario { get; set; }
        public int idUsuario_responsable { get; set; }
        public string nombre { get; set; }
        public string nombrePropietario { get; set; }
        public string responsable { get; set; }
        public string codigoObra { get; set; }

        public ICommand PlantillaxObra { get; private set; }

        public ObraxCargoItem()
        {
            PlantillaxObra = new Command(() =>
            {
                // App.ListarObraxCargo.Navigation.PushAsync(new ListarPlantillaxObra(this));
            });
        }
    }
}
