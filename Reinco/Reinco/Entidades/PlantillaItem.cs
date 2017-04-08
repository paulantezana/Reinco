using Reinco.Interfaces.Plantilla;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class PlantillaItem: INotifyPropertyChanged
    {
        public int idPlantilla { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public bool selecionado { get; set; }
        public bool Selecionado {
            set
            {
                if (selecionado != value)
                {
                    selecionado = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Selecionado"));
                }
            }
            get { return selecionado; }
        }

        #region=============comandos===============
        public ICommand EditarPlantilla { get; private set; }
        public ICommand Actividad { get; private set; }
        #endregion

        #region =============== Constructor(editar plantilla) ====================
        public PlantillaItem()
        {
            EditarPlantilla = new Command(() =>
            {
                App.ListarPlantilla.Navigation.PushAsync(new AgregarPlantilla(this.idPlantilla, this.codigo, this.nombre));
            });
            Actividad = new Command(() =>
            {
                App.ListarPlantilla.Navigation.PushAsync(new ListarActividad(this.idPlantilla,this.nombre));
            });
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
