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

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AsignarPlantilla : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;

        int IdPlantillaPropietarioObra;
        WebService Servicio = new WebService();
        string Mensaje;


        public ObservableCollection<PlantillaItem> Plantillas { get; set; }

        #region ======================== Refreshings ========================
        public bool plantillaIsEnabled { get; set; }
        public bool PlantillaIsEnabled
        {
            set {
                if (plantillaIsEnabled != value)
                {
                    plantillaIsEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlantillaIsEnabled"));
                }
            }
            get { return plantillaIsEnabled; }
        }
        public bool isRefreshingPlantilla { get; set; }
        public bool IsRefreshingPlantilla
        {
            set
            {
                if (isRefreshingPlantilla != value)
                {
                    isRefreshingPlantilla = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingPlantilla"));
                }
            }
            get { return isRefreshingPlantilla; }
        }

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
        #endregion

        #region ======================== Comandos ========================
        public ICommand RefreshPlantillaCommand { get; private set; }
        #endregion

        #region ======================== Constructor ========================
        public AsignarPlantilla(object idPlantillaPropietarioObra)
        {

            InitializeComponent();
            PlantillaIsEnabled = true;
            IdPlantillaPropietarioObra = Convert.ToInt16(idPlantillaPropietarioObra);

            // platilla
            Plantillas = new ObservableCollection<PlantillaItem>();
            CargarPlantillas();

            // Eventos
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;

            // Comandos
            RefreshPlantillaCommand = new Command(() =>
            {
                Plantillas.Clear();
                CargarPlantillas();
                IsRefreshingPlantilla = false;
            });

            // contexto para los bindings
            this.BindingContext = this;
        } 
        #endregion

        #region ====================== Navegacion Cancelar ======================
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        #endregion

        #region ======================== Guardar Plantillas Seleccionadas ========================
        private async void Guardar_Clicked(object sender, EventArgs e)
        {

            try
            {
                cambiarEstado(false);
                // Obteniedo plantillas eleccionados
                var seleccionados = from plantilla in Plantillas where plantilla.Selecionado == true select plantilla.idPlantilla; // obteniendo seleccionados

                // Validacion
                if (seleccionados.Count() == 0)
                {
                    cambiarEstado(true);
                    await DisplayAlert("Asignar Plantilla", "No se ha seleccionado ninguna plantilla.", "Aceptar");
                    return;
                }
                // serializando
                var idSeleccionados = JsonConvert.SerializeObject(seleccionados);
                
                string ids = idSeleccionados.ToString();

                int[] idPlantilla = seleccionados.ToArray();
                int tamaño = seleccionados.Count() + 1;
                object[] idPlantillas = new object[seleccionados.Count()];
                object[,] variables = new object[tamaño, seleccionados.Count()];
                string identificador = "idPlantilla";
                int j = 0;
                for (int i = 0; i < seleccionados.Count(); i++)
                {
                    int numero = Convert.ToInt16(idPlantilla[i]);
                    for (int k = 0; k < seleccionados.Count()-1; k++)
                    {
                        variables[i, k] = identificador;
                        j++;
                        variables[i, j] = numero;
                        j = 0;
                    }
                } // Desde aqui logica para enviar al web service
                variables[seleccionados.Count(), 0] ="idPropietarioObra";
                variables[seleccionados.Count(), 1] = IdPlantillaPropietarioObra;
                var cliente = new HttpClient();
                dynamic result = await Servicio.MetodoPost("ServicioPlantillaPropietarioObra.asmx", "IngresarPlantillaPropietarioObra", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    cambiarEstado(true);
                    await DisplayAlert("Asignar Plantilla", Mensaje, "Aceptar");
                    App.ListarObraPlantilla.ObraPlantillaItems.Clear();
                    App.ListarObraPlantilla.CargarPlantillaObra();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
                cambiarEstado(false);
            }
        } 
        #endregion

        #region ======================== Cargar Plantillas ========================
        private async void CargarPlantillas()
        {
            try
            {
                dynamic plantillas = await Servicio.MetodoGet("ServicioPlantilla.asmx", "MostrarPlantillas");
                foreach (var plantilla in plantillas)
                {
                    Plantillas.Add(new PlantillaItem
                    {
                        idPlantilla = plantilla.idPlantilla,
                        codigo = plantilla.codigo,
                        nombre = plantilla.nombre,
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        } 
        #endregion

        #region Cambiar Estado de los botones y el efecto cargar
        public void cambiarEstado(bool estado)
        {
            PlantillaIsEnabled = estado;
            guardar.IsEnabled = estado;
            if (estado == true)
            {
                IsRunning = false;
            }
            else
            {
                IsRunning = true;
            }
        } 
        #endregion

    }
}
