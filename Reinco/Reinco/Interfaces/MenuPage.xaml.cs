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
        public ListView menuListView { get { return listView; } } // Metodo para cargar los menus
        public MenuPage()
        {
            InitializeComponent();
            var items = new List<MenuPrincipalItem>();


            if (Application.Current.Properties.ContainsKey("cargoUsuario")) // condicional que busca si cargo usuario exite este valo fue almacenado el iniciar sesión
            {


                string cargo = Application.Current.Properties["cargoUsuario"].ToString(); // Recuperando el cargo string y alamacenando en una variable cargo


                #region  ========================== Menu Prfil Importante para cada tipo de usuario ==========================
                    items.Add(new MenuPrincipalItem
                    {
                        Titulo = "Perfil",
                        Icono = "ic_profile.png",
                        TargetType = typeof(PaginaUsuario)
                    });
                #endregion
               


                #region ==========================  Menu Visible solo para los supervisores ==========================
                    if (cargo == "Administrador")
                    {
                        items.Add(new MenuPrincipalItem
                        {
                            Titulo = "Obras",
                            Icono = "ic_obra.png",
                            TargetType = typeof(ListarObra)
                        });
                        items.Add(new MenuPrincipalItem
                        {
                            Titulo = "Personal",
                            Icono = "ic_personal.png",
                            TargetType = typeof(ListarPersonal)
                        });
                        items.Add(new MenuPrincipalItem
                        {
                            Titulo = "Plantillas",
                            Icono = "ic_plantilla.png",
                            TargetType = typeof(PaginaPlantilla)
                        });
                        items.Add(new MenuPrincipalItem
                        {
                            Titulo = "Propietario",
                            Icono = "ic_propietario.png",
                            TargetType = typeof(ListarPropietario)
                        });
                    }

                #endregion



                #region ========================== Menu Visible solo para los Responsables ==========================
                    
                #endregion


                listView.ItemsSource = items; // listando el menu
                
            }
            
        }

        private void cerrarSesion(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }

    internal interface ICloseApplication
    {
        
    }
}
