using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObraPlantilla : ContentPage
    {
        public ObraPlantilla()
        {
            InitializeComponent();
        }
        #region=======================eliminar obra====================================
        public async void eliminar(object sender, EventArgs e)
        {
            var idObra = ((MenuItem)sender).CommandParameter;
            int IdObra = Convert.ToInt16(idObra);
            bool respuesta = await DisplayAlert("Eliminar", "Eliminar idObra = " + idObra, "Aceptar", "Cancelar");
            
        }
        #endregion
        #region ===================// Modificar Obra CRUD //====================
        public void actualizar(object sender, EventArgs e)
        {
            var idObra = ((MenuItem)sender).CommandParameter;
            // Navigation.PushAsync(new AgregarObra(idObra));
        }
        #endregion
    }
}
