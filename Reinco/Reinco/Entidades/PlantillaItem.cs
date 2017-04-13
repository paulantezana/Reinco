using Reinco.Interfaces.Plantilla;
using Reinco.Recursos;
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
        public event PropertyChangedEventHandler PropertyChanged;
        WebService Servicio = new WebService();

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

        public ICommand EditarPlantilla { get; private set; }
        public ICommand Actividad { get; private set; }
        public ICommand Eliminar { get; private set; }

        public PlantillaItem()
        {
            // Modificar
            EditarPlantilla = new Command(() =>
            {
                App.ListarPlantilla.Navigation.PushAsync(new AgregarPlantilla(this.idPlantilla, this.codigo, this.nombre));
            });

            // Actividades
            Actividad = new Command(() =>
            {
                App.ListarPlantilla.Navigation.PushAsync(new ListarActividad(this));
            });

            // Eliminar
            Eliminar = new Command(() =>
            {
                eliminar();
            });
            // End
        }

        public async void eliminar()
        {
            try
            {
                bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Desea eliminar esta plantilla? ", "Aceptar", "Cancelar");
                if (respuesta)
                {
                    object[,] variables = new object[,] { { "idPlantilla", idPlantilla } };
                    dynamic result = await Servicio.MetodoGetString("ServicioPlantilla.asmx", "EliminarPlantilla", variables);
                    string Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        // await mensaje.MostrarMensaje("Eliminar Plantilla", Mensaje);
                        await App.Current.MainPage.DisplayAlert("Eliminar Plantilla", Mensaje, "OK");
                        App.ListarPlantilla.PlantillaItems.Clear();
                        App.ListarPlantilla.CargarPlantilla();
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Eliminar Plantilla", "Error en el dispositivo o URL incorrecto: " + ex.ToString(), "Aceptar");
            }
        }
    }
}
