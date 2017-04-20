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
        WebService Servicio = new WebService();

        ObraItem obra;
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
        public AsignarPlantilla(ObraItem Obra)
        {
            InitializeComponent();
            PlantillaIsEnabled = true;
            directorio.Text = App.directorio + "\\" + Obra.nombre + "\\Agregar Plantillas";
            obra = Obra;
            // platilla
            Plantillas = new ObservableCollection<PlantillaItem>();
            CargarPlantillas(Obra.idPropietarioObra);

            // Eventos
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;

            // Comandos
            RefreshPlantillaCommand = new Command(() =>
            {
                Plantillas.Clear();
                CargarPlantillas(Obra.idPropietarioObra);
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
                //object[,] variables = new object[tamaño, seleccionados.Count()];
                object[,] variables = new object[tamaño,2];
                if (seleccionados.Count()==1)
                     variables = new object[,] { { "idPlantilla", idPlantilla[0] }, { "idPropietarioObra", obra.idPropietarioObra } };
                else {
                    string identificador = "idPlantilla";
                    int j = 0;
                    for (int i = 0; i < seleccionados.Count(); i++)
                    {
                        int numero = Convert.ToInt16(idPlantilla[i]);
                        for (int k = 0; k < seleccionados.Count() - 1; k++)
                        {
                            variables[i, k] = identificador;
                            j++;
                            variables[i, j] = numero;
                            j = 0;
                        }
                    } 
                    // Desde aqui logica para enviar al web service
                    variables[seleccionados.Count(), 0] = "idPropietarioObra";
                    variables[seleccionados.Count(), 1] = obra.idPropietarioObra;
                    var cliente = new HttpClient();
                }
                
                dynamic result = await Servicio.MetodoPostString("ServicioPlantillaPropietarioObra.asmx", "IngresarPlantillaPropietarioObra", variables);
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
                cambiarEstado(true);
            }
        } 
        #endregion

        #region ======================== Cargar Plantillas ========================
        private async void CargarPlantillas(int idPropietarioObra)
        {
            try
            {
                IsRefreshingPlantilla = true;
                object[,] variables = new object[,] {
                        { "idPropietarioObra",idPropietarioObra  }  };
                dynamic plantillas = await Servicio.MetodoGet("ServicioPlantillaPropietarioObra.asmx", "MostrarPlantillasSinAsignar",variables);
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
            finally
            {
                IsRefreshingPlantilla = false;
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
