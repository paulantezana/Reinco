using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModificarObra : ContentPage, INotifyPropertyChanged
    {
        int IdObra;
        int IdPropietario;
        int IdResponsabe;
        int IdPropietarioObra;

        WebService Servicio = new WebService();
        string Mensaje;
        public VentanaMensaje mensaje;

        new public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PropietarioItem> propietarioItems { get; set; }
        public ObservableCollection<PersonalItem> personalItems { get; set; }

        public ICommand commandBorrarPropietario { get; private set; }
        public ICommand commandBorrarResponsable { get; private set; }
        public ICommand guardar { get; set; }

        #region ======================= IsRunnings =======================
        public bool isRunning { get; set; }
        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get { return isRunning; }
        }
        public bool isRunningPropietario { get; set; }
        public bool IsRunningPropietario
        {
            set
            {
                if (isRunningPropietario != value)
                {
                    isRunningPropietario = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunningPropietario"));
                }
            }
            get { return isRunningPropietario; }
        }
        public bool isRunningUsuario { get; set; }
        public bool IsRunningUsuario
        {
            set
            {
                if (isRunningUsuario != value)
                {
                    isRunningUsuario = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunningUsuario"));
                }
            }
            get { return isRunningUsuario; }
        }
        #endregion

        public ModificarObra(int idObra, string Codigo, string Nombre, int idPropietario, int idResponsable, int idPropietarioObra,string nombrePropietario, string nombresApellidos)
        {
            // Preparando la UI(Interfas de usuario) MODIFICAR OBRA
            InitializeComponent();
            this.Title = Nombre; // nombre de la pagina
            nombre.Text = Nombre; // Lenando el campo Nombre Obra
            codigo.Text = Codigo; // llenando el campo Codigo Obra
            lblPropietario.Text = "Asigne un propietario " + App.opcional;
            lblResponsable.Text = "Asigne un responsable " + App.opcional;
            // Variables Globales
            IdObra = Convert.ToInt16(idObra);
            IdPropietario = idPropietario;
            IdResponsabe = idResponsable;
            IdPropietarioObra = idPropietarioObra;

            // Placeholders
            asignarPropietario.Title = nombrePropietario; // Titulo POP UPS Propietario
            asignarResponsable.Title = nombresApellidos; // Titulo POP UPS Responsable

            // Colecciones
            propietarioItems = new ObservableCollection<PropietarioItem>();
            personalItems = new ObservableCollection<PersonalItem>();

            // Cargando las listas
            listarBindablePicker();

            //asignarPropietario.

            // Comandos
            commandBorrarPropietario = new Command(() =>
            {
                asignarPropietario.SelectedValue = 0;
                DisplayAlert("Mensaje", "Se eliminó al propietario.", "Aceptar");
                asignarPropietario.Title = "";
            });
            commandBorrarResponsable = new Command(() =>
            {
                asignarResponsable.SelectedValue = 0;
                DisplayAlert("Mensaje", "Se eliminó al responsable.", "Aceptar");
                asignarResponsable.Title = "";
            });

            guardar = new Command(() =>
            {
                int enviarPropietario = idPropietario;
                int enviarResponsable = idResponsable;

                object propietarioSelecionado = asignarPropietario.SelectedValue;
                object responsableSelecionado = asignarResponsable.SelectedValue;

                if (propietarioSelecionado != null)
                {
                    enviarPropietario = Convert.ToInt16(propietarioSelecionado);
                }
                if (responsableSelecionado != null)
                {
                    enviarResponsable = Convert.ToInt16(responsableSelecionado);
                }
                ModificarPropietarioResponsableObra(enviarPropietario, enviarResponsable);
            });
            // Valor Por Defecto en las listas
            asignarResponsable.Focus();
            asignarPropietario.SelectedValue = idPropietario;
            asignarResponsable.SelectedValue = IdResponsabe;
            // Definiendo costeto para los bindings
            this.BindingContext = this;
        }

        #region ============================ Cargar Propietario ============================
        private async Task CargarPropietarioItem()
        {
            try
            {
                propietarioItems.Clear();
                dynamic propietario = await Servicio.MetodoGet("ServicioPropietario.asmx", "MostrarPropietarios");
                foreach (var item in propietario)
                {
                    propietarioItems.Add(new PropietarioItem
                    {
                        idPropietario = item.idPropietario,
                        nombre = item.nombre
                    });
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error", ex.Message);
            }
        }
        #endregion

        #region ============================ Cargar Personal ============================
        private async Task CargarPersonalItem()
        {
            try
            {
                personalItems.Clear();
                dynamic usuarios = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuariosResponsables");
                foreach (var item in usuarios)
                {
                    personalItems.Add(new PersonalItem
                    {
                        idUsuario = item.idUsuario,
                        nombresApellidos = item.nombresApellidos.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error", ex.Message);
            }
        }
        #endregion

        #region ============================ Modificar Obra ============================
        public async void ModificarPropietarioResponsableObra(int IdPropietario, int IdResponsable)
        {
            try
            {
                cambiarEstado(false);
                object[,] variables = new object[,] { { "codigoObra", codigo.Text }, { "nombreObra", nombre.Text },
                { "IdObra", IdObra },{ "IdPropietario", IdPropietario}, { "IdResponsable", IdResponsable},
                { "IdPropietarioObra", IdPropietarioObra}};
                dynamic result = await Servicio.MetodoGetString("ServicioPropietarioObra.asmx", "ModificarPropietarioObra", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    cambiarEstado(true);
                    await App.Current.MainPage.DisplayAlert("Modificar Obra Propietario y Responsable", Mensaje, "Aceptar");
                    App.ListarObra.ObraItems.Clear();
                    App.ListarObra.CargarObraItems();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await DisplayAlert("Error: ", ex.Message, "Aceptar");
            }
            finally
            {
                cambiarEstado(true);
            }
        } 
        #endregion

        #region ============================= Listar ==============================
        public async void listarBindablePicker()
        {
            try
            {
                IsRunningPropietario = true;
                IsRunningUsuario = true;
                await CargarPersonalItem();
                await CargarPropietarioItem();

                asignarPropietario.ItemsSource = propietarioItems;
                asignarResponsable.ItemsSource = personalItems;
                IsRunningPropietario = false;
                IsRunningUsuario = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        } 
        #endregion

        #region ============================== Cambiar Estado ==============================
        public void cambiarEstado(bool estado)
        {
            codigo.IsEnabled = estado;
            nombre.IsEnabled = estado;
            asignarResponsable.IsEnabled = estado;
            asignarPropietario.IsEnabled = estado;
            guardarCambios.IsEnabled = estado;
            cancelar.IsEnabled = estado;
            if (estado == true) { IsRunning = false; }
            else { IsRunning = true; }
        }
        #endregion
    }
}