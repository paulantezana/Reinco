using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Paginas.Personal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPersonal : ContentPage
    {
        public PaginaPersonal()
        {
            InitializeComponent();
            agregarPersonal.Clicked += AgregarPersonal_Clicked;
        }

        private void AgregarPersonal_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPersonal());
        }
    }
}
