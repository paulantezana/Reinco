using Newtonsoft.Json;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPlantilla : ContentPage
    {
        DialogService dialog;
        public PaginaPlantilla()
        {
            InitializeComponent();
            agregarPlantilla.Clicked += AgregarPlantilla_Clicked;
            //CargarPlantillas();
        }

        private void AgregarPlantilla_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPlantilla());
        }
        
    }
}
