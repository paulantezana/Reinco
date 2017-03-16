using Reinco.Paginas.Supervision;
using System;
using System.Collections.Generic;
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
        public PaginaUsuario()
        {
            InitializeComponent();
            paginaSupervision.Clicked += PaginaSupervision_Clicked;
        }

        private void PaginaSupervision_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PaginaSupervision());
        }
    }
}
