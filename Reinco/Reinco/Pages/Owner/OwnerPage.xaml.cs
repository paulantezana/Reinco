using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Pages.Owner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OwnerPage : ContentPage
    {
        public OwnerPage()
        {
            InitializeComponent();
            addOwner.Clicked += AddOwner_Clicked;
        }

        private void AddOwner_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddOwnerPage());
        }
    }
}
