using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Personal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPersonal : ContentPage
    {
        public ObservableCollection<PersonalItem> personalItem { get; set; }
        
        public PaginaPersonal()
        {
            InitializeComponent();
            personalItem = new ObservableCollection<PersonalItem>();
            CargarPersonalItem();
            personalListView.ItemsSource = personalItem;
            agregarPersonal.Clicked += AgregarPersonal_Clicked;
        }

        private void CargarPersonalItem()
        {
            for (int i = 0; i < 10; i++)
            {
                personalItem.Add(new PersonalItem
                {
                    fotoPerfil = "icon.png",
                    nombre = "Juan",
                    cargo1 = "gernte",
                    cargo1Tareas = "(3)",
                    cargo2 = "Supervisor",
                    cargo2Tareas = "(1)"
                });
            }
        }

        private void AgregarPersonal_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPersonal());
        }
    }
}
