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

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarPlantillaSupervision : ContentPage, INotifyPropertyChanged
    {



        #region +---- Atributos ----+
        public VentanaMensaje mensaje;
        private bool isRefreshingObraPlantilla { get; set; }
        #endregion




        #region +---- Services ----+
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        #endregion




        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion




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

            this.BindingContext = this;
        }

        private async void CargarPlantillaSupervision()
        {
            try
            {
                for (int i = 0; i < 15; i++)
                {
                    PlantillaSupervisionItems.Add(new PlantillaSupervisionItem
                    {
                        nombre = "nombre" + i.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error: ", ex.Message);
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
