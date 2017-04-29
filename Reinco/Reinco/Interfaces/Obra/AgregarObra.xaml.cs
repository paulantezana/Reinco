using System.Collections;
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
        int nroElementos = 10000;// sin funcionalidad para bindable picker(pendiente)
        int ultimoId = 10000;
        int IdAsistente;
        public ObservableCollection<PropietarioItem> propietarioItem {get; set; }
        public ObservableCollection<PersonalItem> personalItem { get; set; }
        public ObservableCollection<PersonalItem> asistenteItem { get; set; }

        #region =============================== Comandos ===============================
        public ICommand commandBorrarPropietario { get; private set; }
        public ICommand commandBorrarResponsable { get; private set; }
        public ICommand commandBorrarAsistente { get; private set; }
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
            directorio.Text = App.directorio + "/Obra/Crear Obra";
            this.Title = "Crear Obra"; // nombre de la pagina

            lblPropietario.Text = "Asigne un propietario " + App.opcional;
            lblResponsable.Text="Asigne un responsable "+ App.opcional;
            lblAsistente.Text="Asigne un Asistente" + App.opcional; 

            // Servicios
            mensaje = new VentanaMensaje();

            // ObservableCollection
            propietarioItem = new ObservableCollection<PropietarioItem>();
            personalItem = new  ObservableCollection<PersonalItem>();
            asistenteItem = new ObservableCollection<PersonalItem>(); ;

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
            commandBorrarAsistente= new Command(() =>
            {
                asignarAsistente.SelectedValue = 0;
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
        }//==============
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
        public bool isRunningAsistente { get; set; }
        public bool IsRunningAsistente
        {
            set
            {
                if (isRunningAsistente != value)
                {
                    isRunningAsistente = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunningAsistente"));
                }
            }
            get { return isRunningAsistente; }
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
        private async Task CargarAsistenteItem()
        {
            try
            {
                dynamic usuarios = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuariosAsistentes");
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
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        private async Task CargarPropietarioItem(int elementos, int ultimo)
        {
            try
            {
                object[,] variables = new object[,] { { "nroElementos", elementos }, { "ultimoId", ultimo } };
                dynamic propietario = await Servicio.MetodoGet("ServicioPropietario.asmx", "MostrarPropietarios",variables);
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
                IsRunningAsistente = true;
                await CargarPersonalItem();
                await CargarAsistenteItem();
                await CargarPropietarioItem(nroElementos,ultimoId);
                asignarAsistente.ItemsSource = asistenteItem;
                asignarPropietario.ItemsSource = propietarioItem;
                asignarResponsable.ItemsSource = personalItem;
                IsRunningPropietario = false;
                IsRunningUsuario = false;
                IsRunningAsistente = false;
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
                        await App.Current.MainPage.DisplayAlert("Agregar Obra", "Rellene todos los campos", "OK");
                        return;
                        
                    }
                    asignarPropietario.InputTransparent = true;
                    object[,] variables = new object[,] { { "codigo", codigo.Text }, { "nombreObra", nombre.Text } };
                    dynamic result = await Servicio.MetodoGetString("ServicioObra.asmx", "IngresarObra", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        cambiarEstado(true);
                        guardar.IsEnabled = false;
                        await App.Current.MainPage.DisplayAlert("Agregar Obra",Mensaje, "OK");
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
                    if (string.IsNullOrEmpty(codigo.Text) || string.IsNullOrEmpty(nombre.Text))
                    {
                        cambiarEstado(true);
                        await App.Current.MainPage.DisplayAlert("Agregar Obra", "Rellene todos los campos", "OK");
                        return;

                    }
                    if (asignarPropietario.SelectedValue != null && asignarResponsable.SelectedValue!=null&&asignarAsistente!=null)
                    {

                        IdPropietario = Convert.ToInt16(asignarPropietario.SelectedValue);
                        IdResponsabe = Convert.ToInt16(asignarResponsable.SelectedValue);
                        IdAsistente = Convert.ToInt16(asignarAsistente.SelectedValue);
                        IngresarPropResponsable(IdPropietario, IdResponsabe,IdAsistente);
                    }
                    else {
                        if (asignarPropietario.SelectedValue == null && asignarResponsable.SelectedValue != null&&asignarAsistente!=null)
                        {
                            IdResponsabe = Convert.ToInt16(asignarResponsable.SelectedValue);
                            IdAsistente= Convert.ToInt16(asignarAsistente.SelectedValue);
                            IngresarPropResponsable(0, IdResponsabe,IdAsistente);
                        }
                        if (asignarPropietario.SelectedValue != null && asignarResponsable.SelectedValue == null && asignarAsistente.SelectedValue == null)
                        {
                            IdPropietario = Convert.ToInt16(asignarPropietario.SelectedValue);
                            IngresarPropResponsable(IdPropietario, 0,0);
                        }
                        if (asignarPropietario.SelectedValue != null && asignarResponsable.SelectedValue == null && asignarAsistente.SelectedValue != null)
                        {
                            IdPropietario = Convert.ToInt16(asignarPropietario.SelectedValue);
                            IdAsistente= Convert.ToInt16(asignarAsistente.SelectedValue);
                            IngresarPropResponsable(IdPropietario, 0, IdAsistente);
                        }
                        if (asignarPropietario.SelectedValue != null && asignarResponsable.SelectedValue != null && asignarAsistente.SelectedValue == null)
                        {
                            IdPropietario = Convert.ToInt16(asignarPropietario.SelectedValue);
                            IdResponsabe = Convert.ToInt16(asignarResponsable.SelectedValue);
                            IngresarPropResponsable(IdPropietario, IdResponsabe, 0);
                        }
                        if (asignarPropietario.SelectedValue == null && asignarResponsable.SelectedValue != null && asignarAsistente.SelectedValue == null)
                        {
                            
                            IdResponsabe = Convert.ToInt16(asignarResponsable.SelectedValue);
                            IngresarPropResponsable(0, IdResponsabe, 0);
                        }
                        if (asignarPropietario.SelectedValue == null && asignarResponsable.SelectedValue == null && asignarAsistente.SelectedValue != null)
                        {

                            IdAsistente = Convert.ToInt16(asignarAsistente.SelectedValue);
                            IngresarPropResponsable(0, 0,IdAsistente );
                        }
                        if (asignarPropietario.SelectedValue == null && asignarResponsable.SelectedValue == null && asignarAsistente.SelectedValue == null)
                        {

                            IngresarPropResponsable(0, 0, 0);
                        }
                        cambiarEstado(true);
                        guardar.IsEnabled = false;
                        await App.Current.MainPage.DisplayAlert("Agregar Obra", Mensaje, "OK");
                        return;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await App.Current.MainPage.DisplayAlert("Agregar Obra", "Error en el dispositivo o URL incorrecto: " + ex.ToString(), "OK");
                return;
            }
            finally
            {
                cambiarEstado(true);
            }
        }
        #endregion
        #region ================================ Scroll Infinito ================================

        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = asignarPropietario.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                CargarPropietarioItem(nroElementos, ultimoId);//------el ultimo id que se recoge
            }
        }
        #endregion
        #region ====================== Ingresar Propietario, Responsable y Asistente ======================
        public async void IngresarPropResponsable(object idPropietario, object idUsuario,object idAsistente)
        {
            cambiarEstado(false);
            object[,] variables = new object[,] { { "codigoObra", codigo.Text }, { "nombreObra", nombre.Text },
                           { "idPropietario",  idPropietario }, { "idUsuarioResponsable", idUsuario} , { "idUsuarioAsistente", idAsistente} };
            dynamic result = await Servicio.MetodoGetString("ServicioPropietarioObra.asmx", "IngresarPropietarioResponsableEnObra", variables);
            Mensaje = Convert.ToString(result);
            if (result != null)
            {
                cambiarEstado(true);
                guardar.IsEnabled = false;
                await App.Current.MainPage.DisplayAlert("Agregar Obra con Responsable, Propietario y Asistente", Mensaje, "OK");
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
            asignarAsistente.IsEnabled = estado;
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
