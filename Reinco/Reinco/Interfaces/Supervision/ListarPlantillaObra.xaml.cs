using System;
using System.Collections.Generic;
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
        public ListarPlantillaObra()
        {
            InitializeComponent();
            plantillaObraListView.ItemsSource = "OBRA";
        }
    }
}
