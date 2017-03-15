using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Pages.Doing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DoingPage : ContentPage
    {
        public DoingPage()
        {
            InitializeComponent();
            addDoing.Clicked += AddDoing_Clicked;
        }

        private void AddDoing_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddDoingPage());
        }
    }
}
