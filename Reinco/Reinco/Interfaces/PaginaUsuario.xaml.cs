using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaUsuario : ContentPage
    {
        // public ObservableCollection<SupervisionItem> tareaUsuarioItems { get; set; }
        public PaginaUsuario()
        {
            InitializeComponent();
            // tareaUsuarioItems = new ObservableCollection<TareaUsuarioItems>();
            //tareaUsuarioListView.ItemsSource = tareaUsuarioItems;
            // tareaUsuarioListView.IsPullToRefreshEnabled = true;
            //CargarTareaUsuarioItems();

            // Cargando datos de usuario logeado
            if (Application.Current.Properties.ContainsKey("nombreUsuario"))
            {
                nombreUsuario.Text = Application.Current.Properties["nombreUsuario"].ToString();
            }
            if (Application.Current.Properties.ContainsKey("apellidoUsuario"))
            {
                apellidoUsuario.Text = Application.Current.Properties["apellidoUsuario"].ToString();
            }
            if (Application.Current.Properties.ContainsKey("cargoUsuario"))
            {
                cargoUsuario.Text = Application.Current.Properties["cargoUsuario"].ToString();
            }
            // --
        }
        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }       
        //private void CargarTareaUsuarioItems()
        //{
        //    for (int i = 0; i < 50; i++)
        //    {
        //        tareaUsuarioItems.Add(new TareaUsuarioItems
        //        {
        //            titulo = "Titulo De La Tarea",
        //            descripcion = "Descripcion De La Tarea",
        //            numeroTarea = Convert.ToString(i),
        //            id = Convert.ToString(i),
        //        });
        //    }
        //}
    }
}
