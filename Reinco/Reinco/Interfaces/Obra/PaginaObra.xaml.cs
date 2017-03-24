using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        // ===================// Iteracion Para Mostrar Obra //====================//
        private void cargarObraItem()
        {
            for (int i = 0; i < 50; i++)
            {
                obraItem.Add(new ObraItem
                {
                    idObra = Convert.ToString(i),
                    nombre = "Nombre De La Obra",
                    codigo = "C0854",
                    responsable = "responsable",
                    platilla = "plantilla"
                });
            }
        }

        // ===================// Navegar A la página AgregarObra.xaml //====================//
        private void AgregarObra_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarObra());
        }

        // ===================// Modificar Obra CRUD //====================//
        public void modificar(object sender, EventArgs e)
        {
            // var mi = ((TapGestureRecognizer)sender);
            // DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
            Navigation.PushAsync(new AgregarObra(e));
        }
        
        // END
    }
}
