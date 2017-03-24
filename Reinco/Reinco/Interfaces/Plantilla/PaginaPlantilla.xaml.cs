
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

        DialogService dialog;
        //private object plantillaListView;

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
            var client = new HttpClient();
            var result = await client.GetAsync("http://192.168.1.37:80/ServicioPlantilla.asmx/MostrarPlantillas");
            //recoge los datos json y los almacena en la variable resultado
            var resultado = await result.Content.ReadAsStringAsync();
            //si todo es correcto, muestra la pagina que el usuario debe ver
            dynamic array = JsonConvert.DeserializeObject(resultado);

            foreach (var item in array)
            {
                plantillaLista.Add(new PlantillaLista
                {
                    codigo = item.nombre.Tostring(),
                    nuemroItems = "15",
                    agregarActividad = "Items"
                });
            }
           
        }

        private void AgregarPlantilla_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPlantilla());
        }
        
    }
}
