using Reinco.Gestores;
using Reinco.Interfaces.Supervision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaUsuario : ContentPage
    {
        public ObservableCollection<TareaUsuarioItems> tareaUsuarioItems { get; set; }
        public PaginaUsuario()
        {
            InitializeComponent();
            tareaUsuarioItems = new ObservableCollection<TareaUsuarioItems>();
            tareaUsuarioListView.ItemsSource = tareaUsuarioItems;
            tareaUsuarioListView.IsPullToRefreshEnabled = true;
            supervisar.Clicked += Supervisar_Clicked;
            CargarTareaUsuarioItems();
        }


        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }
        
        public void OnUpdate(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " update context action", "OK");
        }


        private void Supervisar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PaginaSupervision());
        }

        private void CargarTareaUsuarioItems()
        {
            for (int i = 0; i < 50; i++)
            {
                tareaUsuarioItems.Add(new TareaUsuarioItems
                {
                    titulo = "Titulo De La Tarea",
                    descripcion = "Descripcion De La Tarea",
                    numeroTarea = Convert.ToString(i),
                    id = Convert.ToString(i),
                });
            }
        }
    }
}
