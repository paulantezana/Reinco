using Reinco.Gestores;
using Reinco.Interfaces.Obra;
using Reinco.Interfaces.Personal;
using Reinco.Interfaces.Plantilla;
using Reinco.Interfaces.Propietario;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace Reinco.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage, ICloseApplication
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

            if (Application.Current.Properties.ContainsKey("cargoUsuario"))
            {
                string cargoUsuario = Application.Current.Properties["cargoUsuario"].ToString();
                if (cargoUsuario == "Administrador")
                {
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
                }
                
            }
            
            listView.ItemsSource = items;
            
        }
        public void cerrarAplicacin()
        {
            App.Current.MainPage = new LoginPage();
        }
    }

    internal interface ICloseApplication
    {
    }
}
