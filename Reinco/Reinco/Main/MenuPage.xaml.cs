using Reinco.Models;
using Reinco.Pages.Doing;
using Reinco.Pages.Owner;
using Reinco.Pages.Staff;
using Reinco.Pages.Template;
using Reinco.Pages.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public ListView menuListView { get { return listView; } }
        public MenuPage()
        {
            InitializeComponent();

            var items = new List<MenuPageItem>();
            items.Add(new MenuPageItem
            {
                Title = "Perfil",
                IconSource = "icon.png",
                TargetType = typeof(UserPage)
            });
            items.Add(new MenuPageItem
            {
                Title = "Obras",
                IconSource = "icon.png",
                TargetType = typeof(DoingPage)
            });
            items.Add(new MenuPageItem
            {
                Title = "Personal",
                IconSource = "icon.png",
                TargetType = typeof(StaffPage)
            });
            items.Add(new MenuPageItem
            {
                Title = "Plantillas",
                IconSource = "icon.png",
                TargetType = typeof(TemplatePage)
            });
            items.Add(new MenuPageItem
            {
                Title = "Propietario",
                IconSource = "icon.png",
                TargetType = typeof(OwnerPage)
            });
            listView.ItemsSource = items;

        }
    }
}
