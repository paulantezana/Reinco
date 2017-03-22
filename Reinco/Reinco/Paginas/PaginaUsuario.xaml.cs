using Reinco.Models;
using Reinco.Paginas.Supervision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaUsuario : ContentPage
    {
        public ObservableCollection<TareaUsuarioItems> tareaUsuarioItems { get; set; }
        public PaginaUsuario()
        {
            InitializeComponent();
            tareaUsuarioItems = new ObservableCollection<TareaUsuarioItems>();
            CargarTareaUsuarioItems();
            tarea.ItemsSource = tareaUsuarioItems;
        }

        private void CargarTareaUsuarioItems()
        {
            for (int i = 0; i < 50; i++)
            {
                tareaUsuarioItems.Add(new TareaUsuarioItems
                {
                    titulo = "Titulo De La Tarea",
                    descripcion = "Descripcion De La Tarea",
                    numeroTarea = "67"
                });
            }
        }
    }
}
