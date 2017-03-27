using Newtonsoft.Json;
using Reinco.Recursos;
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
    public partial class PaginaPlantilla : ContentPage
    {
        public ObservableCollection<PlantillaLista> plantillaLista { get; set; }

        public PaginaPlantilla()
        {
            InitializeComponent();
            agregarPlantilla.Clicked += AgregarPlantilla_Clicked;
            plantillaLista = new ObservableCollection<PlantillaLista>();
            CargarPlantillaLista();
            plantillaListView.ItemsSource = plantillaLista;
            
        }

        private async void CargarPlantillaLista()
        {
            try
            {
                var client = new HttpClient();
                var result = await client.GetAsync("http://192.168.1.37:8080/ServicioPlantilla.asmx/MostrarPlantillas");
                //recoge los datos json y los almacena en la variable resultado
                var resultado = await result.Content.ReadAsStringAsync();
                //si todo es correcto, muestra la pagina que el usuario debe ver
                dynamic array = JsonConvert.DeserializeObject(resultado);
                foreach (var item in array)
                {
                    plantillaLista.Add(new PlantillaLista
                    {
                        idPlantilla = item.idPlantilla,
                        codigo = item.codigo,
                        nombre = item.nombre,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error:", ex.Message, "Aceptar");
            }
           
        }

        private void AgregarPlantilla_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPlantilla());
        }

        // ===================// Eliminar Plantilla CRUD //====================// eliminar
        public void eliminar(object sender, EventArgs e)
        {
            var idPlantilla = ((MenuItem)sender).CommandParameter;
            DisplayAlert("Eliminar", "Eliminar idPlantilla = " + idPlantilla, "Aceptar");
        }

        // ===================// Modificar Plantilla CRUD //====================// actualizar
        public void actualizar(object sender, EventArgs e)
        {
            var idPlantilla = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new AgregarPlantilla(idPlantilla));
        }

        // ===================// Modificar Plantilla CRUD //====================// actividades
        public void actividades(object sender, EventArgs e)
        {
            var idPlantilla = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new PaginaActividad(idPlantilla));
        }
        // END ==
    }
}
