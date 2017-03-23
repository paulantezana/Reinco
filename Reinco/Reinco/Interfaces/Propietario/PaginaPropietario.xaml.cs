using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Propietario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPropietario : ContentPage
    {
        public ObservableCollection<PropietarioItem> propietarioItem { get; set; }
        public PaginaPropietario()
        {
            InitializeComponent();
            propietarioItem = new ObservableCollection<PropietarioItem>();
            CargarPropietarioItem();
            propietarioListView.ItemsSource = propietarioItem;
            agregarPropietario.Clicked += AgregarPropietario_Clicked;
        }

        private void CargarPropietarioItem()
        {
            for (int i = 0; i < 20; i++)
            {
                propietarioItem.Add(new PropietarioItem
                {
                    fotoPerfil = "icon.png",
                    nombre = "Perico"
                });
            }
        }

        private void AgregarPropietario_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPropietario());
        }
    }
}
