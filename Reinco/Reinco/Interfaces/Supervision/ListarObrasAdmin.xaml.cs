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
    public partial class ListarObrasAdmin : ContentPage, INotifyPropertyChanged
    {
        #region +---- Atributos ----+
        public VentanaMensaje mensaje;
        private bool RefrescandoObra { get; set; }
        #endregion

        #region +---- Services ----+
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        #endregion

        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +---- Propiedades ----+
        public ObservableCollection<ObraItem> ObraItems { get; set; }
        public bool RefrescandoObras
        {
            set
            {
                if (RefrescandoObra != value)
                {
                    RefrescandoObra = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RefrescandoObras"));
                }
            }
            get
            {
                return RefrescandoObra;
            }
        }
        #endregion

        #region +---- Comandos ----+
        public ICommand RefrescandoObrasCommand { get; private set; }
        #endregion

        public ListarObrasAdmin()
        {
            //InitializeComponent();
            InitializeComponent();
            mensaje = new VentanaMensaje();
            ObraItems = new ObservableCollection<ObraItem>();
            CargarObrasItem();

            RefrescandoObrasCommand = new Command(() =>
            {
                ObraItems.Clear();
                CargarObrasItem();
                RefrescandoObras = false;
            });

            this.BindingContext = this; // Contexto de los Bindings Clase Actual Importante para que pueda funcionar el refresco de la lista con Gestos
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarObrasAdmin = this;
        }

        private async void CargarObrasItem()
        {
            try
            {
                //servicioObra, mostrarObras--modificado
                dynamic obras = await Servicio.MetodoGet("ServicioPropietarioObra.asmx", "MostrarPropietarioObraDetalle");
                foreach (var item in obras)
                {
                    if (item.idPropietario == null || item.idUsuario_responsable == null)
                        Color = "#FF7777";
                    else
                        Color = "#77FF77";
                    ObraItems.Add(new ObraItem
                    {
                        idObra = item.idObra,
                        nombre = item.nombre,
                        codigo = item.codigo,
                        idPropietario = item.idPropietario == null ? 0 : item.idPropietario,
                        idUsuario = item.idUsuario_responsable == null ? 0 : item.idUsuario_responsable,
                        colorObra = Color,
                        idPropietarioObra = item.idPropietario_Obra == null ? 0 : item.idPropietario_Obra
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
