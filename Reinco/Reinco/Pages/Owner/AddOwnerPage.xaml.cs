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
    public partial class AddOwnerPage : ContentPage
    {
        public AddOwnerPage()
        {
            InitializeComponent();
            cancel.Clicked += Cancel_Clicked;
            save.Clicked += Save_Clicked;
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
