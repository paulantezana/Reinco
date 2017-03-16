using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Models
{
    public class MainViewModel
    {

        #region Properties
        public ObservableCollection<PlantillaItem> plantillaItem { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            plantillaItem = new ObservableCollection<PlantillaItem>();
            CargarPlantillaItem();
        }
        #endregion

        #region Metodos
        private void CargarPlantillaItem()
        {
            for (int i = 1; i < 11; i++)
            {
                plantillaItem.Add(new PlantillaItem
                {
                    item = Convert.ToString(i),
                    actividad = "Previo: Condicion Bomba",
                    aprobacionSi = true,
                    aprobacionNo = false,
                    observacionLevantada = false
                });
            }
        } 
        #endregion

    }
}
