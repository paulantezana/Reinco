using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private void CargarPlantillaLista()
        {
            for (int i = 0; i < 15; i++)
            {
                plantillaLista.Add(new PlantillaLista
                {
                    codigo = "35",
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
