using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearSupervision : ContentPage, INotifyPropertyChanged
    {
        #region==========atributos====================
        private bool isRunning;
        int IdPlantillaObra;
        WebService Servicio = new WebService();
        int IdSupervision;
        string Mensaje;
        public VentanaMensaje mensaje;
        #endregion

        new public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<PersonalItem> personalItem { get; set; }
        #region +---- Refrescando ----+
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
            get
            {
                return isRunning;
            }
        }
        #endregion
        public CrearSupervision(PlantillaSupervisionItem plantilla)//-----------Modificar supervision
        {
            InitializeComponent();
            personalItem = new ObservableCollection<PersonalItem>();
            IdSupervision =plantilla.idSupervision;
            lblNroSupervision.Text = "Número de Supervisión";
            lblPartidaEvaluada.Text = "Partida Evaluada " + App.opcional;
            lblNivel.Text = "Nivel " + App.opcional;
            lblBloque.Text = "Bloque " + App.opcional;
            CargarPersonalItem();
            asignarAsistente.ItemsSource = personalItem;
            lblFecha.Text =plantilla.fecha.ToString() +" (fecha creada)";
            numeroSupervision.Text = plantilla.numero.ToString();
            partidaEvaluada.Text = plantilla.partidaEvaluada;
            bloque.IsToggled = plantilla.bloque==1?true:false;
            nivel.Text = plantilla.nivel;
            asignarAsistente.Title = plantilla.nombreAsistente;
            if (App.cargo == "Asistente")
                asignarAsistente.IsEnabled = false;
            guardar.Clicked += Modificar_Supervision;
        }

        private async void Modificar_Supervision(object sender, EventArgs e)//--Modificar la supervision
        {
            try
            {

                int nroSupervision = Convert.ToInt16(numeroSupervision.Text);

                var fechatemp = fecha.Date.ToString("dd/MM/yyyy");

                object[,] variables = new object[,] { { "idSupervision",IdSupervision}
                    , { "fecha", fecha.Date.ToString("dd/MM/yyyy") },
                { "nroSupervision", nroSupervision }, { "partidaEvaluada", partidaEvaluada.Text==null?"":partidaEvaluada.Text },
                { "bloque", bloque.IsToggled?1:0 }, { "nivel", nivel.Text==null?"":nivel.Text },{ "idAsistente", asignarAsistente.SelectedValue==null?0:asignarAsistente.SelectedValue }};
                dynamic result = await Servicio.MetodoGetString("ServicioSupervision.asmx", "ModificarSupervisionCreada", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    guardar.IsEnabled = false;
                    await App.Current.MainPage.DisplayAlert("Modificar Supervisión", Mensaje, "OK");
                    App.ListarPlantillaSupervision.PlantillaSupervisionItems.Clear();
                    App.ListarPlantillaSupervision.CargarPlantillaSupervision();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Modificar Supervisión ", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }

        public CrearSupervision(int idPlantillaObra)
        {
            InitializeComponent();
            personalItem = new ObservableCollection<PersonalItem>();
            IdPlantillaObra = idPlantillaObra;
            lblNroSupervision.Text = "Número de Supervisión";
            lblPartidaEvaluada.Text="Partida Evaluada " + App.opcional;
            lblNivel.Text="Nivel " + App.opcional;
            lblBloque.Text = "Bloque " + App.opcional;
            CargarPersonalItem();
            asignarAsistente.Title = App.nombreUsuarioAsistente; // Titulo POP UPS Responsable
            asignarAsistente.SelectedValue = App.idUsuarioAsistente;
            if (App.cargo == "Asistente")
                asignarAsistente.IsEnabled = false;
            asignarAsistente.ItemsSource = personalItem;
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;
            
            // lblNroSupervision.Text = App.ultimoNroSupervision.ToString();

            numeroSupervision.Text = (App.ultimoNroSupervision+1).ToString();
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {

                int nroSupervision = Convert.ToInt16(numeroSupervision.Text);

                var fechatemp = fecha.Date.ToString("dd/MM/yyyy");

                object[,] variables = new object[,] { { "idSupervisor", asignarAsistente.SelectedValue==null?0:asignarAsistente.SelectedValue },
                    { "idPlantillaPropietario", IdPlantillaObra }, { "fecha", fecha.Date.ToString("dd/MM/yyyy") },
                { "nroSupervision", nroSupervision }, { "partidaEvaluada", partidaEvaluada.Text==null?"":partidaEvaluada.Text },
                { "bloque", bloque.IsToggled?1:0 }, { "nivel", nivel.Text==null?"":nivel.Text }};
                dynamic result = await Servicio.MetodoGetString("ServicioSupervision.asmx", "CrearSupervision", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    guardar.IsEnabled = false;
                    await App.Current.MainPage.DisplayAlert("Crear Supervisión", Mensaje, "OK");
                    App.ListarPlantillaSupervision.PlantillaSupervisionItems.Clear();
                    App.ListarPlantillaSupervision.CargarPlantillaSupervision();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Crear Supervisión", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador","Ok");
            }
            App.ultimoNroSupervision = 0;
        }

       

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    App.ListarObraPlantilla = this;
        //}
        #region Navegacion para el boton cancelar
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        #endregion
        #region +=================== Cargando Usuarios Desde Web Service ===========================+
        private async void CargarPersonalItem()
        {
            try
            {
                personalItem.Clear();
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
                await mensaje.MostrarMensaje("Error", ex.Message);
            }
        }
        #endregion
    }
}
