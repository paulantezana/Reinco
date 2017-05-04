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
        WebService Servicio = new WebService();
        string Mensaje;
        ObraItem obra;
        int nroElementos = 10000;// sin funcionalidad para bindable picker(pendiente)
        int ultimoId = 10000;
        new public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PropietarioItem> propietarioItems { get; set; }
        public ObservableCollection<PersonalItem> personalItems { get; set; }
        public ObservableCollection<PersonalItem> asistenteItems { get; set; }
        public ICommand commandBorrarPropietario { get; private set; }
        public ICommand commandBorrarResponsable { get; private set; }
        public ICommand commandBorrarAsistente { get; private set; }
        public ICommand guardar { get; set; }
       // public ICommand finalizada { get; set; }

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

        public ModificarObra(ObraItem Obra)
        {
            // Preparando la UI(Interfas de usuario) MODIFICAR OBRA
            InitializeComponent();
            obra = Obra;

            this.Title = Obra.nombre;       // nombre de la pagina
            nombre.Text = Obra.nombre;      // Lenando el campo Nombre Obra
            codigo.Text = Obra.codigo;           // llenando el campo Codigo Obra

            lblPropietario.Text = "Asigne un propietario " + App.opcional;
            lblResponsable.Text = "Asigne un responsable " + App.opcional;
            lblAsistente.Text= "Asigne un Asistente " + App.opcional;
            Sfinalizada.IsToggled = Obra.finalizada != 1 ? false : true;
            // Variables Globales

            // Placeholders
            asignarPropietario.Title = Obra.nombrePropietario; // Titulo POP UPS Propietario
            asignarResponsable.Title = Obra.nombresApellidos; // Titulo POP UPS Responsable
            asignarAsistente.Title = Obra.nombreAsistente;
            // Colecciones
            propietarioItems = new ObservableCollection<PropietarioItem>();
            personalItems = new ObservableCollection<PersonalItem>();
            asistenteItems = new ObservableCollection<PersonalItem>();
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
            commandBorrarAsistente = new Command(() =>
            {
                asignarAsistente.SelectedValue = 0;
                DisplayAlert("Mensaje", "Se eliminó al asistente.", "Aceptar");
                asignarAsistente.Title = "";
            });

            guardar = new Command(() =>
            {
                int enviarPropietario = Obra.idPropietario;
                int enviarResponsable = Obra.idUsuario;
                int enviarAsistente = obra.idAsistente;
                object propietarioSelecionado = asignarPropietario.SelectedValue;
                object responsableSelecionado = asignarResponsable.SelectedValue;
                object asistenteSeleccionado = asignarAsistente.SelectedValue;
                if (propietarioSelecionado != null)
                {
                    enviarPropietario = Convert.ToInt16(propietarioSelecionado);
                }
                if (responsableSelecionado != null)
                {
                    enviarResponsable = Convert.ToInt16(responsableSelecionado);
                }
                if (asistenteSeleccionado != null)
                {
                    enviarAsistente = Convert.ToInt16(asistenteSeleccionado);
                }
                ModificarPropietarioResponsableObra(enviarPropietario, enviarResponsable,enviarAsistente);
            });
            // Valor Por Defecto en las listas
            asignarResponsable.Focus();
            asignarPropietario.SelectedValue = Convert.ToInt16(Obra.idPropietario);
            asignarResponsable.SelectedValue = Obra.idUsuario;
            asignarAsistente.SelectedValue = Obra.idAsistente;
            // Definiendo costeto para los bindings
            this.BindingContext = this;
        }


        #region ============================ Modificar Obra ============================
        public async void ModificarPropietarioResponsableObra(int IdPropietario, int IdResponsable,int IdAsistente)
        {
            try
            {
                cambiarEstado(false);
                
                object[,] variables = new object[,] { { "codigoObra", codigo.Text }, { "nombreObra", nombre.Text },
                { "IdObra", obra.idObra },{ "IdPropietario", IdPropietario}, { "IdResponsable", IdResponsable},
                { "IdPropietarioObra", obra.idPropietarioObra}, { "supervisionTerminada", Sfinalizada.IsToggled==true?1:0}, { "idUsuarioAsistente",  IdAsistente}};
                dynamic result = await Servicio.MetodoGetString("ServicioPropietarioObra.asmx", "ModificarPropietarioObra", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    cambiarEstado(true);
                    guardarCambios.IsEnabled = false;
                    await App.Current.MainPage.DisplayAlert("Modificar Obra Propietario, Responsable y Asistente", Mensaje, "Aceptar");
                    App.ListarObra.ObraItems.Clear();
                    App.ListarObra.CargarObraItems();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await DisplayAlert("Error: ", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
            }
            finally
            {
                cambiarEstado(true);
            }
        }
        #endregion

        #region ============================= Listar ==============================
        // Cargar Personal
        private async Task CargarPropietarioItem(int elementos, int ultimo)
        {
            try
            {
                propietarioItems.Clear();
                object[,] variables = new object[,] { { "nroElementos", elementos }, { "ultimoId", ultimo } };
                dynamic propietario = await Servicio.MetodoGet("ServicioPropietario.asmx", "MostrarPropietarios",variables);
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
                await DisplayAlert("Error propietario", ex.Message,"Aceptar");
            }
        }
        // Cargar Personal
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
                await DisplayAlert("Error", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
            }
        }
        private async Task CargarAsistenteItem()
        {
            try
            {
                asistenteItems.Clear();
                dynamic usuarios = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuariosAsistentes");
                foreach (var item in usuarios)
                {
                    asistenteItems.Add(new PersonalItem
                    {
                        idUsuario = item.idUsuario,
                        nombresApellidos = item.nombresApellidos.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        public async void listarBindablePicker()
        {
            try
            {
                IsRunningPropietario = true;
                IsRunningUsuario = true;
                await CargarPersonalItem();
                await CargarPropietarioItem(nroElementos, ultimoId);
                await CargarAsistenteItem();
                asignarPropietario.ItemsSource = propietarioItems;
                asignarResponsable.ItemsSource = personalItems;
                asignarAsistente.ItemsSource = asistenteItems;
                IsRunningPropietario = false;
                IsRunningUsuario = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
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