using Reinco.Interfaces.Obra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Reinco.Recursos;
using Reinco.Interfaces.Plantilla;
using Reinco.Interfaces.Supervision;

namespace Reinco.Entidades
{
    public class ObraItem
    {
        WebService Servicio = new WebService();

        public int idObra { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public int idPropietario { get; set; }
        public int idUsuario { get; set; }
        public string plantilla { get; set; }
        public string colorObra { get; set; }
        public int idPropietarioObra { get; set; }
        public int idPlantillaObra { get; set; }
        public string nombrePropietario { get; set; }
        public string nombresApellidos { get; set; }
        public bool ocultar { get; set; }

        public ICommand asignarPlantilla { get; private set; }
        public ICommand editarObra { get; private set; }
        public ICommand Eliminar { get; private set; }
        public ICommand mostrarPlantillas { get; private set; }


        public ObraItem()
        {
            ocultar = true;
            // Editar Obra
            editarObra = new Command(() =>
            {
                App.ListarObra.Navigation.PushAsync(new ModificarObra(this));
            });

            // Mostrar Plantillas
            mostrarPlantillas = new Command(() =>
            {
                App.ListarObra.Navigation.PushAsync(new ListarObraPlantilla(this));
            });

            // Eliminar Obra
            Eliminar = new Command(() =>
            {
                eliminar();
            });
        }

        #region ==================================== Eliminar Obra ====================================
        private async void eliminar()
        {
            try
            {
                bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Desea eliminar la obra:  " + this.nombre + " ? ", "Aceptar", "Cancelar");
                if (!respuesta) return;

                object[,] variables = new object[,] { { "idPropietarioObra", idPropietarioObra }, { "idObra", idObra } };
                dynamic result = await Servicio.MetodoGetString("ServicioPropietarioObra.asmx", "EliminarPropietarioObra", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Obra", Mensaje, "OK");
                    App.ListarObra.ObraItems.Clear();
                    App.ListarObra.CargarObraItems();
                    return;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Eliminar Obra", "Error en el dispositivo o URL incorrecto: " + ex.Message, "Aceptar");
            }
        } 
        #endregion
    }
}
