using Reinco.Interfaces;
using Reinco.Interfaces.Supervision;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reinco.Entidades
{
    public class ObraPlantillaItem
    {
        WebService Servicio = new WebService();

        public string nombre { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int idPropietarioObra { get; set; }
        public int idPlantillaObra { get; set; }
        public int idObra { get; set; }
        public int idPlantilla { get; set; }
        public string colorPlantilla { get; set; }
        public bool verEliminar { get; set; }
        public ICommand PlantillaSupervision { get; private set; }
        public ICommand Eliminar { get; private set; }



        public ObraPlantillaItem()
        {
            PlantillaSupervision = new Command(() =>
            {
                try
                {
                    App.ListarObraPlantilla.Navigation.PushAsync(new ListarPlantillaSupervision(idPlantillaObra,idObra,idPlantilla,nombre));
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Error: ",  ex.Message, "Aceptar");
                }
            });
            Eliminar = new Command(() =>
            {
                eliminar();
            });
        }

        private async void eliminar()
        {
            try
            {
                bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Desea eliminar esta plantilla de la obra? \n " + this.nombre, "Aceptar", "Cancelar");
                if (!respuesta) return;

                object[,] variables = new object[,] { { "idPlantillaPropietarioObra", this.idPlantillaObra } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantillaPropietarioObra.asmx", "EliminarPlantillaPropietarioObra", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Plantilla Obra", Mensaje, "Aceptar");
                    App.ListarObraPlantilla.ObraPlantillaItems.Clear();
                    App.ListarObraPlantilla.CargarPlantillaObra();
                    return;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Eliminar Plantilla Obra", "Error en el dispositivo o URL incorrecto: " + ex.Message,"Aceptar");
            }
        }
    }
}
