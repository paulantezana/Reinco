using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class ListarPlantillaObra : ContentPage
    {
        public ObservableCollection<PlantillaObra> plantillaObra { get; set; }
        public ListarPlantillaObra()
        {
            InitializeComponent();
            plantillaObra = new ObservableCollection<PlantillaObra>();

            CargarPlantillaObra();
            plantillaObraListView.ItemsSource = plantillaObra;
        }

        private void CargarPlantillaObra()
        {
            for (int i = 0; i < 200; i++)
            {
                plantillaObra.Add(new PlantillaObra
                {
                    nombre = "Gestion De Calidad" + Convert.ToString(i),
                    codigo = "F" + Convert.ToString(i),
                    descripcion = "Brebe Descripcion" + Convert.ToString(i)
                });
            }
        }
    }
}
