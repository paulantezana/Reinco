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
    public partial class AgregarObra : ContentPage
    {

        // ============================ Constructor para crear obra ============================//
        public AgregarObra()
        {
            InitializeComponent();
            cancelar.Clicked += Cancelar_Clicked1;
        }

        #region Navegacion para el voton cancelar
        // boton Cancelar de crear obra
        private void Cancelar_Clicked1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        // boton cancelar de modificar y eliminar obra
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        } 
        #endregion

        // ============== Constructor para para modificar o eliminar la  obra ===============//
        public AgregarObra(object e)
        {
            InitializeComponent();
            cancelar.Clicked += Cancelar_Clicked;
            guardar.Text = "Guardar Cambios";
        }
    }
}
