using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaActividad : ContentPage
    {
        public ObservableCollection<ActividadItems> actividadItems { get; set; }
        public PaginaActividad(object idPlantilla)
        {
            InitializeComponent();
            actividadItems = new ObservableCollection<ActividadItems>();
            CargarActividadItems();
            actividadListView.ItemsSource = actividadItems;

            // eventos
            agregarActividad.Clicked += AgregarActividad_Clicked;
        }

        private void AgregarActividad_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarActividad());
        }

        private void CargarActividadItems()
        {
            byte x = 01; // utilizada para la enumeracion de las actividades

            for (int i = 0; i < 11; i++)
            {
                actividadItems.Add(new ActividadItems
                {
                    idActividad = 8,
                    nombre = "Nombre de la actividad",
                    tolerancia = "Tolerancia",
                    enumera = x++,
                });
            }
        }
        // ===================// Eliminar Plantilla CRUD //====================// eliminar
        public void eliminar(object sender, EventArgs e)
        {
            var idActividad = ((MenuItem)sender).CommandParameter;
            DisplayAlert("Eliminar", "Eliminar idActividad = " + idActividad, "Aceptar");
        }

        // ===================// Modificar Plantilla CRUD //====================// actualizar
        public void actualizar(object sender, EventArgs e)
        {
            var idActividad = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new AgregarActividad(idActividad));
        }
    }
}
