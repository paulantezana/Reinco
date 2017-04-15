using Newtonsoft.Json;
using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarObra : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PropietarioItem> propietarioItem {get; set; }
        public ObservableCollection<PersonalItem> personalItem { get; set; }

        #region =============================== Comandos ===============================
        public ICommand commandBorrarPropietario { get; private set; }
        public ICommand commandBorrarResponsable { get; private set; }
        #endregion

        #region ============================== Atributos ==============================
        int IdObra;
        int IdPropietario;
        int IdResponsabe;
        int IdPropietarioObra;
        WebService Servicio = new WebService();
        string Mensaje;
        public VentanaMensaje mensaje;
        #endregion

        #region +==================Constructores=====================+
        public AgregarObra()
        {
            // Preparando la UI(Interfas de usuario)
            InitializeComponent();
            directorio.Text = App.directorio + "\\Obra\\Crear Obra";
            this.Title = "Crear Obra"; // nombre de la pagina

            lblPropietario.Text = "Asigne un propietario " + App.opcional;
            lblResponsable.Text="Asigne un responsable "+ App.opcional;

            // Servicios
            mensaje = new VentanaMensaje();

            // ObservableCollection
            propietarioItem = new ObservableCollection<PropietarioItem>();
            personalItem = new  ObservableCollection<PersonalItem>();

            // Cargando las listas en los POP UPS
            listarBindablePicker();

            // Eventos
            cancelar.Clicked += Cancelar_Clicked;
            guardar.Clicked += Guardar_Clicked;

            // Comandos
            commandBorrarPropietario = new Command(() =>
            {
                asignarPropietario.SelectedValue = 0;
            });
            commandBorrarResponsable = new Command(() =>
            {
                asignarResponsable.SelectedValue = 0;
            });

            // Contexto para los bindings
            this.BindingContext = this;
        }
        #endregion

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

        #region =========================== listar BindablePicker ===========================
        private async Task CargarPersonalItem()
        {
            try
            {
                dynamic usuarios = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuariosResponsables");
                foreach (var item in usuarios)
                {
                    personalItem.Add(new PersonalItem
                    {
                        idUsuario = item.idUsuario,
                        nombresApellidos = item.nombresApellidos.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message,"Aceptar");
            }
        }

        private async Task CargarPropietarioItem()
        {
            try
            {
                dynamic propietario = await Servicio.MetodoGet("ServicioPropietario.asmx", "MostrarPropietarios");
                foreach (var item in propietario)
                {
                    propietarioItem.Add(new PropietarioItem
                    {
                        idPropietario = item.idPropietario,
                        nombre = item.nombre
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
                await CargarPropietarioItem();

                asignarPropietario.ItemsSource = propietarioItem;
                asignarResponsable.ItemsSource = personalItem;
                IsRunningPropietario = false;
                IsRunningUsuario = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        } 
        #endregion

        #region ============================= Guardar Nueva Obra =============================
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                cambiarEstado(false);
                if (asignarPropietario.SelectedValue == null && asignarResponsable.SelectedValue == null)
                {
                    #region================ingresar solo obra=============================
                    if (string.IsNullOrEmpty(codigo.Text) || string.IsNullOrEmpty(nombre.Text))
                    {
                        cambiarEstado(true);
                        await mensaje.MostrarMensaje("Agregar Obra", "Debe rellenar todos los campos.");
                        return;
                        
                    }
                    asignarPropietario.InputTransparent = true;
                    object[,] variables = new object[,] { { "codigo", codigo.Text }, { "nombreObra", nombre.Text } };
                    dynamic result = await Servicio.MetodoGetString("ServicioObra.asmx", "IngresarObra", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        cambiarEstado(true);
                        await mensaje.MostrarMensaje("Agregar Obra", Mensaje);
                        
                        // Refrescando la lista
                        App.ListarObra.ObraItems.Clear();
                        App.ListarObra.CargarObraItems();
                        await Navigation.PopAsync();
                        return;
                    }
                    #endregion
                }

                #region===========ingresar con responsable y propietario=============
                else
                {
                    if (asignarPropietario.SelectedValue != null && asignarResponsable.SelectedValue!=null)
                    {

                        IdPropietario = Convert.ToInt16(asignarPropietario.SelectedValue);
                        IdResponsabe = Convert.ToInt16(asignarResponsable.SelectedValue);
                        IngresarPropResponsable(IdPropietario, IdResponsabe);
                    }
                    else {
                        if (asignarPropietario.SelectedValue == null && asignarResponsable.SelectedValue != null)
                        {
                            IdResponsabe = Convert.ToInt16(asignarResponsable.SelectedValue);
                            IngresarPropResponsable(0, IdResponsabe);
                        }
                        if (asignarPropietario.SelectedValue != null && asignarResponsable.SelectedValue == null)
                        {
                            IdPropietario = Convert.ToInt16(asignarPropietario.SelectedValue);
                            IngresarPropResponsable(IdPropietario, 0);
                        }
                        cambiarEstado(true);
                        await mensaje.MostrarMensaje("Agregar Obra con Responsable y Propietario", Mensaje);
                        return;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await mensaje.MostrarMensaje("Agregar Obra", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
            finally
            {
                cambiarEstado(true);
            }
        }
        #endregion

        #region ====================== Ingresar Propietario y Responsable ======================
        public async void IngresarPropResponsable(object idPropietario, object idUsuario)
        {
            cambiarEstado(false);
            object[,] variables = new object[,] { { "codigoObra", codigo.Text }, { "nombreObra", nombre.Text },
                           { "idPropietario",  idPropietario }, { "idUsuarioResponsable", idUsuario} };
            dynamic result = await Servicio.MetodoGetString("ServicioPropietarioObra.asmx", "IngresarPropietarioResponsableEnObra", variables);
            Mensaje = Convert.ToString(result);
            if (result != null)
            {
                cambiarEstado(true);
                await App.Current.MainPage.DisplayAlert("Agregar Obra con Responsable y Propietario", Mensaje, "OK");
                App.ListarObra.ObraItems.Clear();
                App.ListarObra.CargarObraItems();
                await Navigation.PopAsync();
                return;
            }
        }
        #endregion

        #region ============================== Cambiar Estado ==============================
        public void cambiarEstado(bool estado)
        {
            codigo.IsEnabled = estado;
            nombre.IsEnabled = estado;

            asignarPropietario.IsEnabled = estado;
            asignarResponsable.IsEnabled = estado;

            guardar.IsEnabled = estado;
            cancelar.IsEnabled = estado;

            if (estado == true) { IsRunning = false; }
            else { IsRunning = true; }
        }
        #endregion

        #region ============================== Cacelar ==============================
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        #endregion
    }
}
