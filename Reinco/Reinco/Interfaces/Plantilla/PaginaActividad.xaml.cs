using Newtonsoft.Json;
using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaActividad : ContentPage
    {
        int IdPlantilla;
        public ObservableCollection<ActividadItems> actividadItems { get; set; }
        public PaginaActividad(object idPlantilla)
        {
            InitializeComponent();
            IdPlantilla = Convert.ToInt16(idPlantilla);
            actividadItems = new ObservableCollection<ActividadItems>();
            actividadListView.ItemsSource = actividadItems;
            CargarActividadItems();
            // eventos
            agregarActividad.Clicked += AgregarActividad_Clicked;
        }

        private void AgregarActividad_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarActividad(IdPlantilla));
        }

        private async void CargarActividadItems()
        {
            byte x = 01; // utilizada para la enumeracion de las actividades
            
            try
            {
                var client = new HttpClient();
                
                var result = await client.GetAsync("http://192.168.1.37:8080/ServicioPlantillaActividad.asmx/MostrarActividadxIdPlantilla?idPlantilla=" + IdPlantilla);
                //recoge los datos json y los almacena en la variable resultado
                var resultado = await result.Content.ReadAsStringAsync();
                //si todo es correcto, muestra la pagina que el usuario debe ver
                dynamic array = JsonConvert.DeserializeObject(resultado);

                foreach (var item in array)
                {
                    actividadItems.Add(new ActividadItems
                    {
                        idActividad = item.idPlantilla_Actividad,
                        nombre = item.nombre,
                        tolerancia = item.tolerancia,
                        enumera = x++,
                    });
                }

            }
            catch (Exception ex)
            {

                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        // ===================// Eliminar Plantilla CRUD //====================// eliminar
        public async void eliminar(object sender, EventArgs e)
        {
            var idActividad = ((MenuItem)sender).CommandParameter;
            int IdActividad = Convert.ToInt16(idActividad);
            bool respuesta = await DisplayAlert("Eliminar", "Eliminar idObra = " + IdActividad, "Aceptar", "Cancelar");
            using (var cliente = new HttpClient())
            {
                var result = await cliente.GetAsync("http://192.168.1.37:8080/ServicioPlantillaActividad.asmx/EliminarPlantillaActividad?idPlantillaActividad=" + IdActividad);
                var json = await result.Content.ReadAsStringAsync();
                string mensaje = Convert.ToString(json);

                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Actividad", mensaje, "OK");
                    return;
                }
            }
        }

        // ===================// Modificar Plantilla CRUD //====================// actualizar
        public void actualizar(object sender, EventArgs e)
        {
            var idActividad = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new AgregarActividad(idActividad,IdPlantilla));
        }
    }
}
