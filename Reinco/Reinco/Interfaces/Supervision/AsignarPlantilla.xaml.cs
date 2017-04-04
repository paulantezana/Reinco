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
    public partial class AsignarPlantilla : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;

        int IdPlantillaPropietarioObra;
        WebService Servicio = new WebService();
        string Mensaje;


        public ObservableCollection<PlantillaItem> plantillaLista { get; set; }


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
            get
            {
                return isRunning;
            }
        }
        public AsignarPlantilla(object idPlantillaPropietarioObra)
        {
            InitializeComponent();
            this.Title = "Plantillas disponibles";
            IdPlantillaPropietarioObra = Convert.ToInt16(idPlantillaPropietarioObra);
            plantillaLista = new ObservableCollection<PlantillaItem>();

            CargarPlantillaLista();

            // Eventos
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;

            // listar
            asignarPlantilla.ItemsSource = plantillaLista;
            this.BindingContext = this;
        }

        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {

            try
            {
                cambiarEstado(false);
                if (asignarPlantilla.SelectedValue == null)
                {
                    cambiarEstado(true);
                    await DisplayAlert("Asignar Plantilla", "Debe Asignar alguna plantilla", "Aceptar");
                    return;
                }
                object[,] variables = new object[,] { { "idPlantilla", asignarPlantilla.SelectedValue }, { "idPropietarioObra", IdPlantillaPropietarioObra } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantillaPropietarioObra.asmx", "IngresarPlantillaPropietarioObra", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    cambiarEstado(true);
                    await DisplayAlert("Asignar Plantilla", Mensaje, "Aceptar");

                    // Refrescando la lista
                    // App.ListarObra.ObraItems.Clear();
                    App.ListarObraPlantilla.ObraPlantillaItems.Clear();
                    App.ListarObraPlantilla.CargarPlantillaObra();
                    cambiarEstado(false);
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

        private async void CargarPlantillaLista()
        {
            try
            {
                
                dynamic plantillas = await Servicio.MetodoGet("ServicioPlantilla.asmx", "MostrarPlantillas");
                foreach (var plantilla in plantillas)
                {
                    plantillaLista.Add(new PlantillaItem
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


        #region Cambiar Estado de los botones y el efecto cargar
        public void cambiarEstado(bool estado)
        {
            guardar.IsEnabled = estado;
            asignarPlantilla.IsEnabled = estado;
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
