using Reinco.Gestores;
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
        public ObservableCollection<SupervisionItem> supervisionItem { get; set; } // Metodo de colecion observable para listar Las Obras A Supervisar
        public ObservableCollection<ObraResponsableItem> obraResponsableItem { get; set; } // Metodo de observable para listar la obras a cargo del responsable
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
                    if (Application.Current.Properties.ContainsKey("nombreUsuario"))
                    {
                        nombreUsuario.Text = Application.Current.Properties["nombreUsuario"].ToString();
                    }
                    if (Application.Current.Properties.ContainsKey("apellidoUsuario"))
                    {
                        apellidoUsuario.Text = Application.Current.Properties["apellidoUsuario"].ToString();
                    }
                    cargoUsuario.Text = cargo;
                #endregion


                #region //============================ Zona Administrador ===================================//
                    if (cargo == "Administrador")
                    {
                        supervisarListView.IsEnabled = false;
                        supervisarListView.IsVisible = false;
                        interfazResponsable.IsVisible = false;
                        resPonsableListView.IsEnabled = false;
                        resPonsableListView.IsVisible = false;
                    }
                #endregion



                #region ++ =============================== Zona Responsable =============================== ++
                    if (cargo == "Responsable")
                    {
                        interfazAdministrador.IsVisible = false;
                        supervisarListView.IsEnabled = false;
                        supervisarListView.IsVisible = false;
                        obraResponsableItem = new ObservableCollection<ObraResponsableItem>();
                        CargarObraResponsableItem();
                        
                        // listando ------------------------------------------ //
                        resPonsableListView.ItemsSource = obraResponsableItem;
                        crearSupervision.Clicked += CrearSupervision_Clicked;
                    }

                #endregion



                #region ** =============================== Zona Supervision =============================== **
                    if (cargo == "Supervision")
                    {
                        interfazAdministrador.IsVisible = false;
                        interfazResponsable.IsVisible = false;
                        resPonsableListView.IsEnabled = false;
                        resPonsableListView.IsVisible = false;
                    supervisionItem = new ObservableCollection<SupervisionItem>();
                        // +-----+ Cargando Obras A supervisar +-----+
                        CargarSupervisionItem();
                        // +-----+ Listando la obras a supervisar  +-----+
                        supervisarListView.ItemsSource = supervisionItem;
                    }
                #endregion


            }
        }

       

        #region // =============================== Responsable =============================== //
        private  async void CargarObraResponsableItem()
        {
            try
            {

                // Recuperando el id Usuario
                string recuperarIdUsuario = Application.Current.Properties["idUsuario"].ToString();
                Int16 idUsuario = Convert.ToInt16(recuperarIdUsuario);

                // Iniciando Web Service
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idResponsable", idUsuario } };
                dynamic result = await servicio.MetodoPost("ServicioObra.asmx", "MostrarObrasResponsable", variables);

                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        await mensaje.MostrarMensaje("Mostrar Obra Responsable", "No Hay Obras a su cargo");
                    }
                    else
                    {
                        // listando las obras
                        foreach (var item in result)
                        {
                            obraResponsableItem.Add(new ObraResponsableItem
                            {
                                nombre = item.nombre,
                                idResponsable = item.idObra,
                            });
                        }
                        // fin del listado
                    }
                }
                else
                {
                    await mensaje.MostrarMensaje("Mostrar Obra Responsable","A ocurrido un error al listar las obras para este usuario"); 
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        // Evento Tappet Para las listas
        private async void responsableTapped(object sender, EventArgs e)
        {
            var idResponsable = ((ImageCell)sender).CommandParameter.ToString();
            await mensaje.MostrarMensaje("ms", idResponsable);
        }



        // Crear Supervision
        private void CrearSupervision_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CrearSupervision());
        }

        #endregion


        #region // =============================== Iniciar Supervision =============================== //
        public void supervisar(object sender, EventArgs e)
            {
                var idupervisar = ((MenuItem)sender).CommandParameter.ToString();
                DisplayAlert("Supervisar", idupervisar + " more context action", "OK");
                Navigation.PushAsync(new ListarSupervision());
            } 
        #endregion



        #region // =============================== Cargando Datos Con Iteracion Para Supervision =============================== //
        private void CargarSupervisionItem()
            {
                for (int i = 0; i < 50; i++)
                {
                    supervisionItem.Add(new SupervisionItem
                    {
                        titulo = "Titulo De La Tarea",
                        descripcion = "Descripcion De La Tarea",
                        numeroTarea = Convert.ToString(i),
                        id = Convert.ToString(i),
                    });
                }
            }

        #endregion


        

        private void irObra(object sender, EventArgs e)
        {
            // App.Current.MainPage = new NavigationPage(new PaginaObra());
        }
        private void irPersonal(object sender, EventArgs e)
        {
            // App.Current.MainPage = new NavigationPage(new PaginaPersonal());
        }
        private void irPlantilla(object sender, EventArgs e)
        {
            // App.Current.MainPage = new NavigationPage(new PaginaPlantilla());
        }
        private void irPropietario(object sender, EventArgs e)
        {
            // App.Current.MainPage = new NavigationPage(new PaginaPropietario());
        }
    }
}
