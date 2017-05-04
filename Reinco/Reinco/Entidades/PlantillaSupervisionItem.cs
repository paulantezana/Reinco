using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Reinco.Interfaces.Supervision;
using Reinco.Recursos;

namespace Reinco.Entidades
{
    public class PlantillaSupervisionItem
    {
        WebService Servicio = new WebService();
        public int idSupervision { get; set; }
        public string nombre { get; set; }
        public int numero { get; set; }
        public string fecha { get; set; }
        public int idObra { get; set; }
        public int idPlantilla { get; set; }
        public string partidaEvaluada { get; set; }
        public string nivel { get; set; }
        public string nombreAsistente { get; set; }
        public string nombreResponsable { get; set; }
        public int bloque { get; set; }
        public ICommand Supervisar { get; private set; }
        public ICommand editarSupervision { get; private set; }
        public ICommand verActividades { get; private set; }
        public ICommand eliminar { get; private set; }
        public string colorSupervision { get;set; }
        public string nombreObra { get; set; }
        public string correo { get; set; }


        public PlantillaSupervisionItem()
        {
            Supervisar = new Command(() =>
            { 
                //App.ListarPlantillaSupervision.Navigation.PushAsync(new Supervisar());
            });
            verActividades = new Command(() =>
            {
                App.ListarPlantillaSupervision.Navigation.PushAsync(new Supervisar(idSupervision,nombreObra));
                //App.ListarPlantillaSupervision.Navigation.PushAsync(new Supervisar());
            });
            editarSupervision = new Command(() =>
            {
                App.ListarPlantillaSupervision.Navigation.PushAsync(new CrearSupervision(this));
                //App.ListarPlantillaSupervision.Navigation.PushAsync(new Supervisar());
            });
            eliminar = new Command(() =>
            {
                Eliminar(this.idSupervision);
                //App.ListarPlantillaSupervision.Navigation.PushAsync(new Supervisar());
            });
        }
        public async void Eliminar(int IdSupervision)
        {
            try
            {
                bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Desea eliminar la supervisión: " + this.nombre + "? ", "Aceptar", "Cancelar");
                if (respuesta)
                {
                    object[,] variables = new object[,] { { "idSupervision", idSupervision } };
                    dynamic result = await Servicio.MetodoGetString("ServicioSupervision.asmx", "EliminarSupervision", variables);
                    string Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Eliminar Supervision", Mensaje, "OK");
                        App.ListarPlantillaSupervision.PlantillaSupervisionItems.Clear();
                        App.ListarPlantillaSupervision.CargarPlantillaSupervision();
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Eliminar supervisión", "Sin conexión a internet, inténtelo  nuevamente", "Aceptar");
            }
        }
    }
}
