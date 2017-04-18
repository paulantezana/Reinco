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
        public CrearSupervision(int idPlantillaObra)
        {
            InitializeComponent();
            personalItem = new ObservableCollection<PersonalItem>();
            IdPlantillaObra = idPlantillaObra;
            lblNroSupervision.Text = "Número de Supervisión";
            lblPartidaEvaluada.Text="Partida Evaluada " + App.opcional;
            lblNivel.Text="Nivel " + App.opcional;
            lblBloque.Text="Bloque " + App.opcional;
            CargarPersonalItem();
            asignarAsistente.ItemsSource = personalItem;
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;
            
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {

                if (asignarAsistente.SelectedValue==null)
                {
                    await DisplayAlert("Crear Supervision", "Debe asignar un Asistente.", "OK");
                    return;
                }
                int nroSupervision = Convert.ToInt16(numeroSupervision.Text);

                var fechatemp = fecha.Date.ToString("dd/MM/yyyy");

                object[,] variables = new object[,] { { "idSupervisor", asignarAsistente.SelectedValue },
                    { "idPlantillaPropietario", IdPlantillaObra }, { "fecha", fecha.Date.ToString("dd/MM/yyyy") },
                { "nroSupervision", nroSupervision }, { "partidaEvaluada", partidaEvaluada.Text==null?"":partidaEvaluada.Text },
                { "bloque", bloque.IsToggled?1:0 }, { "nivel", nivel.Text==null?"":nivel.Text }};
                dynamic result = await Servicio.MetodoGetString("ServicioSupervision.asmx", "CrearSupervision", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Agregar Supervision", Mensaje, "OK");
                    //App.ListarActividad.ActividadItems.Clear();
                    App.ListarPlantillaSupervision.PlantillaSupervisionItems.Clear();
                    // App.ListarActividad.CargarActividadItems();
                    App.ListarPlantillaSupervision.CargarPlantillaSupervision();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Supervision", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
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
                dynamic usuarios = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuarios");
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
