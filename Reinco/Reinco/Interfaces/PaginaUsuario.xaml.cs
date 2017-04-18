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
        public Grid Cuadricula { get; set; }

        #region =============================== GRID ===============================
        public void grid()
        {
            this.Cuadricula = new Grid();
            this.Cuadricula.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            this.Cuadricula.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            this.Cuadricula.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            this.Cuadricula.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            this.Cuadricula.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            this.Cuadricula.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }
        #endregion

        #region =============================== Constructores ===============================
        public PaginaUsuario()
        {
            InitializeComponent(); // inicializa todo los componentes de la UI

            if (Application.Current.Properties.ContainsKey("cargoUsuario") && Application.Current.Properties.ContainsKey("idUsuario")) // condisional que busca si cargo usuario exite este valo fue almacenado el iniciar sesión
            {
                string cargo = Application.Current.Properties["cargoUsuario"].ToString(); // Recuperando el cargo string y alamacenando en una variable cargo
                int idUsuario = Convert.ToInt16(Application.Current.Properties["idUsuario"].ToString());

                if (Application.Current.Properties.ContainsKey("nombresApellidos"))
                {
                    nombreUsuario.Text = Application.Current.Properties["nombresApellidos"].ToString();
                }
                cargoUsuario.Text = cargo;
                if (cargo == "Gerente")
                {
                    this.uiAdmin();
                }
                if (cargo == "Responsable")
                {
                    this.uiResponsable(idUsuario);
                }
                if (cargo == "Asistente")
                {
                    this.uiAsistente(idUsuario);
                }
            }
        }
        //  UI Como Perfil de cada usuario
        public PaginaUsuario(PersonalItem Usuario)
        {
            InitializeComponent();
            nombreUsuario.Text = Usuario.nombresApellidos;
            cargoUsuario.Text = Usuario.cargo;

            if (Usuario.cargo == "Gerente")
            {
                this.uiAdmin();
            }
            if (Usuario.cargo == "Responsable")
            {
                this.uiResponsable(Usuario.idUsuario);
            }
            if (Usuario.cargo == "Asistente")
            {
                this.uiAsistente(Usuario.idUsuario);
            }
        } 
        #endregion

        #region =============================== UI ADMIN ===============================
        public void uiAdmin()
        {
            this.grid();
            // Obra
            uiHome obra = new uiHome("ic_obra_color.png", "Obras");
            obra.eventoTap.Command = new Command(() =>
            {
                App.Navigator.Detail = new NavigationPage(new ListarObra());
            });

            // Plantilla
            uiHome plantilla = new uiHome("ic_plantilla_color.png", "Plantilla");
            plantilla.eventoTap.Command = new Command(() =>
            {
                App.Navigator.Detail = new NavigationPage(new ListarPlantilla());
            });

            // Propietario
            uiHome propietario = new uiHome("ic_propietario_color.png", "Propietario");
            propietario.eventoTap.Command = new Command(() =>
            {
                App.Navigator.Detail = new NavigationPage(new ListarPropietario());
            });

            // Personal
            uiHome personal = new uiHome("ic_personal_color.png", "Personal");
            personal.eventoTap.Command = new Command(() =>
            {
                App.Navigator.Detail = new NavigationPage(new ListarPersonal());
            });

            // Posicionando los elementos en la interfas
            Cuadricula.Children.Add(obra.contenedor, 0, 0);
            Cuadricula.Children.Add(plantilla.contenedor, 1, 0);
            Cuadricula.Children.Add(propietario.contenedor, 2, 0);
            Cuadricula.Children.Add(personal.contenedor, 0, 1);

            // Pintando la Interfas
            uixCargo.Children.Add(Cuadricula);
        }
        #endregion

        #region =============================== UI RESPONSABLE ===============================
        public void uiResponsable(int idUsuario)
        {
            this.grid();
            uiHome obraResponsable = new uiHome("ic_obra_color.png", "Obra");
            obraResponsable.eventoTap.Command = new Command(() =>
            {
                Navigation.PushAsync(new ListarObraxCargo(idUsuario, "Responsable"));
                // App.Navigator.Detail = new NavigationPage(new ListarObra());
            });

            // Posicionando los elementos en la interfas
            Cuadricula.Children.Add(obraResponsable.contenedor, 0, 0);

            // Pintando la Interfas
            uixCargo.Children.Add(Cuadricula);
        }
        #endregion

        #region =============================== UI ASISTENTE ===============================
        public void uiAsistente(int idUsuario)
        {
            this.grid();
            uiHome obraAsistente = new uiHome("ic_obra_color.png", "Obra");
            obraAsistente.eventoTap.Command = new Command(() =>
            {
                // Navigation.PushAsync(new ListarObraxCargo(idUsuario, "Asistente"));
                App.Navigator.Detail = new NavigationPage(new ListarObra());
            });

            // Posicionando los elementos en la interfas
            Cuadricula.Children.Add(obraAsistente.contenedor, 0, 0);

            // Pintando la Interfas
            uixCargo.Children.Add(Cuadricula);
        } 
        #endregion
    }
    public class uiHome
    {
        public StackLayout contenedor { get; set; }
        public TapGestureRecognizer eventoTap { get; set; }
        public uiHome(string image, string nombre)
        {
            this.contenedor = new StackLayout();
            this.contenedor.Children.Add(new Image { Source = image });
            this.contenedor.Children.Add(new Label { Text = nombre, HorizontalOptions = LayoutOptions.CenterAndExpand });
            this.eventoTap = new TapGestureRecognizer();
            this.contenedor.GestureRecognizers.Add(this.eventoTap);
        }
    }
}
