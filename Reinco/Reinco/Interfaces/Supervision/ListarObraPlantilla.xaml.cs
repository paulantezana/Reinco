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
    public partial class ListarObraPlantilla : ContentPage, INotifyPropertyChanged
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

        public ObservableCollection<ObraPlantillaItem> ObraPlantillaItems { get; set; }

        public ICommand RefreshObraPlantillaCommand { get; private set; }



        public ListarObraPlantilla()
        {
            InitializeComponent();

            ObraPlantillaItems = new ObservableCollection<ObraPlantillaItem>();


            // Refrescar la lista
            RefreshObraPlantillaCommand = new Command(() =>
            {
                ObraPlantillaItems.Clear();
                CargarPlantillaObra();
            });


            // Cargando la lista
            CargarPlantillaObra();

            // Contexto de los Bindings Clase Actual Importante para que pueda funcionar el refresco de la lista con Gestos
            this.BindingContext = this;  
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarObraPlantilla = this;
        }



        private void CargarPlantillaObra()
        {
            for (int i = 0; i < 200; i++)
            {
                ObraPlantillaItems.Add(new ObraPlantillaItem
                {
                    nombre = "Gestion De Calidad" + Convert.ToString(i),
                    codigo = "F" + Convert.ToString(i),
                    descripcion = "Brebe Descripcion" + Convert.ToString(i)
                });
            }
        }


    }
}
