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
            CargarObraItem();
            obrasListView.ItemsSource = obraItem;

            agregarObra.Clicked += AgregarObra_Clicked;
            
        }

        private void CargarObraItem()
        {
            for (int i = 0; i < 30; i++)
            {
                obraItem.Add(new ObraItem
                {
                    titulo = "Nombre De La Obra",
                    responsable = "nombre del responsable",
                    platilla = "PLANTILLAS"
                });
            }
        }

        private void AgregarObra_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarObra());
        }
    }
}
