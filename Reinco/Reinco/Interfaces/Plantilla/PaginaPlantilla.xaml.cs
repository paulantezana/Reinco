
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
        }
        private void AgregarPlantilla_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPlantilla());
        }
    }
}
