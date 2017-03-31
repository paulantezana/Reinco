using Reinco.Interfaces.Obra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class ObraItem
    {

       
        public int idObra { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string plantilla { get; set; }
        public string colorObra { get; set; }



        #region +---- Comandos ---+
        public ICommand asignarPlantilla { get; private set; }
        public ICommand editarObra { get; private set; }
        public ICommand eliminar { get; private set; }
        
        #endregion


        #region +---- Constructor ----+
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
            eliminar = new Command(() =>
            {
                // Eliminar logica de programacion aqui
                App.Current.MainPage.DisplayAlert("Titulo", this.nombre + this.idObra, "Acpetar");
            });
        } 
        #endregion


    }
}
