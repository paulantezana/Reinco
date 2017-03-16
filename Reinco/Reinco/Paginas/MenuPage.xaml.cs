using Reinco.Models;
using Reinco.Paginas.Obra;
using Reinco.Paginas.Personal;
using Reinco.Paginas.Plantilla;
using Reinco.Paginas.Propietario;
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
    public partial class MenuPage : ContentPage
    {
        public ListView menuListView { get { return listView; } }
        public MenuPage()
        {
            InitializeComponent();

            var items = new List<MenuPrincipalItem>();
            items.Add(new MenuPrincipalItem
            {
                Titulo = "Perfil",
                Icono = "ic_profile.png",
                TargetType = typeof(PaginaUsuario)
            });
            items.Add(new MenuPrincipalItem
            {
                Titulo = "Obras",
                Icono = "ic_profile.png",
                TargetType = typeof(PaginaObra)
            });
            items.Add(new MenuPrincipalItem
            {
                Titulo = "Personal",
                Icono = "ic_profile.png",
                TargetType = typeof(PaginaPersonal)
            });
            items.Add(new MenuPrincipalItem
            {
                Titulo = "Plantillas",
                Icono = "ic_profile.png",
                TargetType = typeof(PaginaPlantilla)
            });
            items.Add(new MenuPrincipalItem
            {
                Titulo = "Propietario",
                Icono = "ic_profile.png",
                TargetType = typeof(PaginaPropietario)
            });
            listView.ItemsSource = items;

        }
    }
}
