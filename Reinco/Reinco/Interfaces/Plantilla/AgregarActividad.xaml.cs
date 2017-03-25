using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarActividad : ContentPage
    {
        public AgregarActividad()
        {
            InitializeComponent();

            // Eventos
            cancelar.Clicked += Cancelar_Clicked;
        }

        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        // ===================// Constructor Actualizar // ===================//
        public AgregarActividad(object idActividad)
        {
            InitializeComponent();
            guardar.Text = "Guardar Cambios";

            // eventos
            guardar.Clicked += GuardarCambios_Clicked;
            cancelar.Clicked += Cancelar_Clicked1;
        }

        // ===================// GuardarCambios // ===================//
        private void GuardarCambios_Clicked(object sender, EventArgs e)
        {
            // aqui logica de programacio para guardar cambios
            DisplayAlert("Guardar cambios", "Se guardo los cambios de forma exitosamente","Aceptar");
            Navigation.PopAsync();
        }
        // ===================// Cancelar // =================== //
        private void Cancelar_Clicked1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        // ===================//
    }
}
