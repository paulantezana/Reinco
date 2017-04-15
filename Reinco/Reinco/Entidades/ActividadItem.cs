using Reinco.Interfaces.Plantilla;
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
    public class ActividadItem
    {
        WebService Servicio = new WebService();

        public int idActividad { get; set; }
        public string nombre { get; set; }
        public string tolerancia { get; set; }
        public byte enumera { get; set; }
        public int idPlantilla { get; set; }

        public ICommand Eliminar { get; private set; }
        public ICommand editarActividad { get; private set; }


        public ActividadItem()
        {
            // Eliminar
            Eliminar = new Command(() =>
            {
                eliminar();
            });

            // Modificar
            editarActividad = new Command(() =>
            {
                App.ListarActividad.Navigation.PushAsync(new AgregarActividad(this));
            });
        }

        public async void eliminar()
        {
            try
            {
                bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Desea eliminar esta Actividad: " + this.nombre + "?", "Aceptar", "Cancelar");
                if (respuesta)
                {
                    object[,] variables = new object[,] { { "idPlantillaActividad", this.idActividad } };
                    dynamic result = await Servicio.MetodoGetString("ServicioPlantillaActividad.asmx", "EliminarPlantillaActividad", variables);
                    string Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Eliminar Actividad", Mensaje, "OK");
                        App.ListarActividad.ActividadItems.Clear();
                        App.ListarActividad.CargarActividad();
                        // await Navigation.PopAsync();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Eliminar Actividad", "Error en el dispositivo o URL incorrecto: " + ex.Message, "Aceptar");
            }
        }
    }
}
