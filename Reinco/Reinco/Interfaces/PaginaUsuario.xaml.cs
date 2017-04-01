using Reinco.Entidades;
using Reinco.Interfaces.Obra;
using Reinco.Interfaces.Personal;
using Reinco.Interfaces.Plantilla;
using Reinco.Interfaces.Propietario;
using Reinco.Interfaces.Supervision;
using Reinco.Recursos;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaUsuario : ContentPage
    {
        public VentanaMensaje mensaje;

        public PaginaUsuario()
        {
            InitializeComponent(); // inicializa todo los componentes de la UI

            // Servicios
            mensaje = new VentanaMensaje(); 


            if (Application.Current.Properties.ContainsKey("cargoUsuario")) // condisional que busca si cargo usuario exite este valo fue almacenado el iniciar sesión
            {
                string cargo = Application.Current.Properties["cargoUsuario"].ToString(); // Recuperando el cargo string y alamacenando en una variable cargo


                #region ++++++++++++++++++++++++++++++++++++++ General ++++++++++++++++++++++++++++++++++++++
                // --------------- Imprimiendo datos del usuario logeado --------------- //


                    if (Application.Current.Properties.ContainsKey("nombresApellidos"))
                    {
                        nombreUsuario.Text = Application.Current.Properties["nombresApellidos"].ToString();
                    }
                    cargoUsuario.Text = cargo;


                #endregion


                #region //============================ Zona Administrador ===================================//
                    if (cargo == "Administrador")
                    {
                        supervisarListView.IsEnabled = false;
                        supervisarListView.IsVisible = false;
                        interfazResponsable.IsVisible = false;
                    }
                #endregion



                #region ++ =============================== Zona Responsable =============================== ++
                    if (cargo == "Responsable")
                    {
                        interfazAdministrador.IsVisible = false;
                        supervisarListView.IsEnabled = false;
                        supervisarListView.IsVisible = false;
                    //App.ListarObras
                    }

                #endregion



                #region ** =============================== Zona Supervision =============================== **
                    if (cargo == "Supervision")
                    {
                        interfazAdministrador.IsVisible = false;
                        interfazResponsable.IsVisible = false;
                    }
                #endregion


            }
        }

        #region //=============================  Navegacion Para el Administrador  =============================//
        private void irObra(object sender, EventArgs e)
        {
            App.Navigator.Detail = new NavigationPage(new ListarObra());
        }
        private void irPersonal(object sender, EventArgs e)
        {
            App.Navigator.Detail = new NavigationPage(new ListarPersonal());
        }
        private void irPlantilla(object sender, EventArgs e)
        {
            App.Navigator.Detail = new NavigationPage(new ListarPlantilla());
        }
        private void irPropietario(object sender, EventArgs e)
        {
            App.Navigator.Detail = new NavigationPage(new ListarPropietario());
        }
        #endregion

        private void irObraResponsable(object sender, EventArgs e)
        {
            App.Navigator.Detail = new NavigationPage(new ListarObras());
        }
    }
}
