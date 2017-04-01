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
                    if (cargo == "Gerente")
                    {
                        //interfazResponsable.IsVisible = false;
                        //interfazSupervisor.IsVisible = false;
                    }
                #endregion



                #region ++ =============================== Zona Responsable =============================== ++
                    if (cargo == "Responsable")
                    {
                        interfazAdministrador.IsVisible = false;
                    }

                #endregion



                #region ** =============================== Zona Supervision =============================== **
                    if (cargo == "Asistente")
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




        // Listar obras que ya tienen un responsable y propietario
        private void irObraAdmin(object sender, EventArgs e)
        {
            // Recuperando el id Usuario   cargoUsuario
            string idUsuario = Application.Current.Properties["idUsuario"].ToString();
            string cargoUsuario = Application.Current.Properties["cargoUsuario"].ToString();
            App.Navigator.Detail = new NavigationPage(new ListarObras(idUsuario, cargoUsuario));
        }
        #endregion




        #region Navegacion Responsable
        // Listar obras que ya tienen un responsable y propietario Solor Del Responsable
        private void irObraResponsable(object sender, EventArgs e)
        {
            // Recuperando el id Usuario   cargoUsuario
            string idUsuario = Application.Current.Properties["idUsuario"].ToString();
            string cargoUsuario = Application.Current.Properties["cargoUsuario"].ToString();
            App.Navigator.Detail = new NavigationPage(new ListarObras(idUsuario, "Responsable"));
        }
        #endregion




        #region Navegacion ObraSupervisor
        // Listar obras que ya tienen un responsable y propietario  Solo Del Supervisor
        private void irObraSupervisor(object sender, EventArgs e)
        {
            // Recuperando el id Usuario   cargoUsuario
            string idUsuario = Application.Current.Properties["idUsuario"].ToString();
            string cargoUsuario = Application.Current.Properties["cargoUsuario"].ToString();
            App.Navigator.Detail = new NavigationPage(new ListarObras(idUsuario, "Supervisor"));
        }
        #endregion



    }
}
