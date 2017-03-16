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

        #endregion

        #region Constructors
        public MainViewModel()
        {
            userTask = new ObservableCollection<UserTaskItems>();
            LoadUserTask();
        }
        #endregion

        #region Methods
        private void LoadUserTask()
        {
            userTask.Add(new UserTaskItems
            {
                title = "Title 1",
                description = "description",
                taskNumber = "25"
            });
        }
        #endregion
    }
}
