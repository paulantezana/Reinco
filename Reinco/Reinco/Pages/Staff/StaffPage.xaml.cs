using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Pages.Staff
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffPage : ContentPage
    {
        public StaffPage()
        {
            InitializeComponent();
            addStaff.Clicked += AddStaff_Clicked;
        }

        private void AddStaff_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddStaffPage());
        }
    }
}
