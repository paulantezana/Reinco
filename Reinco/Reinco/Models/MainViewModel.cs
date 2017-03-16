using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Models
{
    class MainViewModel
    {
        #region Properties
        public ObservableCollection<UserTaskItems> userTask { get; set; }
        // public ObservableCollection<PlantillaItem> plantillaItem { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            userTask = new ObservableCollection<UserTaskItems>();
            LoadUserTask();
            // cargarPlantillaItem();
        }
        #endregion

        #region Methods
        private  void LoadUserTask()
        {
            for (int i = 0; i < 100; i++)
            {
                userTask.Add(new UserTaskItems
                {
                    title = "Obra A Supervisar",
                    description = "Descripcion Corta",
                    taskNumber = Convert.ToString(i)
                });
            }
        }
        /*private void cargarPlantillaItem()
        {
            for (int i = 0; i < 11; i++)
            {
                plantillaItem.Add(new PlantillaItem
                {
                    item = Convert.ToString(i),
                    actividad = "Previo Condición Bomba",
                    aprobacionNo = true,
                    aprobacionSi = false,
                    observacionLevantada = false
                });
            }
        }*/
        #endregion
    }
}
