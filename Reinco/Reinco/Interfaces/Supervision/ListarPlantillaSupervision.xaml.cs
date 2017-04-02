using Reinco.Entidades;
using Reinco.Interfaces.Plantilla;
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

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarPlantillaSupervision : ContentPage, INotifyPropertyChanged
    {



        #region +---- Atributos ----+
        public VentanaMensaje mensaje;
        private bool isRefreshingObraPlantilla { get; set; }
        int IdPlantillaObra;
        #endregion

        #region +---- Services ----+
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        #endregion

        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        public ICommand AgregarSupervision { get; private set; }

        public bool IsRefreshingObraPlantilla
        {
            set
            {
                if (isRefreshingObraPlantilla != value)
                {
                    isRefreshingObraPlantilla = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingObraPlantilla"));
                }
            }
            get
            {
                return isRefreshingObraPlantilla;
            }
        }

        public ObservableCollection<PlantillaSupervisionItem> PlantillaSupervisionItems { get; set; }

        public ICommand RefreshObraPlantillaCommand { get; private set; }

        public ListarPlantillaSupervision()
        {
            InitializeComponent();
            PlantillaSupervisionItems = new ObservableCollection<PlantillaSupervisionItem>();
            CargarPlantillaSupervision();
            // nuevaSupervision.Clicked += NuevaSupervision_Clicked;
            AgregarSupervision = new Command(() =>
            {
                Navigation.PushAsync(new CrearSupervision(IdPlantillaObra));
            });
            this.BindingContext = this;
        }
        public ListarPlantillaSupervision(int idPlantillaObra)
        {
            InitializeComponent();
            IdPlantillaObra = idPlantillaObra;
            PlantillaSupervisionItems = new ObservableCollection<PlantillaSupervisionItem>();
            CargarPlantillaSupervision();
            AgregarSupervision = new Command(() =>
            {
                Navigation.PushAsync(new CrearSupervision(IdPlantillaObra));
            });
            this.BindingContext = this;
        }
        private void NuevaSupervision_Clicked(object sender, EventArgs e)
        {

            throw new NotImplementedException();
        }

        public async void CargarPlantillaSupervision()
        {
            try
            {
                byte x = 01;
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idPlantillaPropObra", IdPlantillaObra } };
                dynamic result = await servicio.MetodoGet("ServicioSupervision.asmx", "SupervisionesxIdPlantillaObra", variables);

                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        //await mensaje.MostrarMensaje("Mostrar Obra Plantilla", "No hay plantillas que mostrar");
                        await DisplayAlert("Supervisiones por plantilla", "No hay plantillas", "Aceptar");
                        return;
                    }
                    else
                    {
                        // listando las obras
                        foreach (var item in result)
                        {
                            PlantillaSupervisionItems.Add(new PlantillaSupervisionItem
                            {
                                nombre="Supervision",
                                numero = x++,
                                idSupervision=item.idSupervision
                            });
                        }
                        // fin del listado
                    }
                }
                else
                {
                    await mensaje.MostrarMensaje("Supervisiones", "Error de respuesta del servicio, Contáctese con el administrador.");
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error:", ex.Message);
            }
        }

        #region Global
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarPlantillaSupervision = this;
        } 
        #endregion



    }
}
