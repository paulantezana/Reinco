using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaObra : ContentPage
    {
        public ObservableCollection<ObraItem> obraItem { get; set; }
        public PaginaObra()
        {
            InitializeComponent();
            obraItem = new ObservableCollection<ObraItem>();
            cargarObraItem();
            obrasListView.ItemsSource = obraItem;
            agregarObra.Clicked += AgregarObra_Clicked;
        }

        private void cargarObraItem()
        {
            for (int i = 0; i < 50; i++)
            {
                obraItem.Add(new ObraItem
                {
                    titulo = "Titulo",
                    responsable = "responsable",
                    platilla = "plantilla"
                });
            }
        }

        private void AgregarObra_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarObra());
        }
    }
}
