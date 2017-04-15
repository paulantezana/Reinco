using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarActividad : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;
        WebService Servicio = new WebService();
        ActividadItem actividad;
        PlantillaItem plantilla;

        #region ========================= IsRunning =========================
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

        public AgregarActividad(PlantillaItem Plantilla)
        {
            InitializeComponent();
            directorio.Text = App.directorio + "\\" + Plantilla.nombre + "\\Agregar Actividad";
            plantilla = Plantilla;

            // Eventos
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;

            // Contexto para los bindings
            this.BindingContext = this;
        }
        public AgregarActividad(ActividadItem Actividad)
        {
            InitializeComponent();
            directorio.Text = App.directorio + "\\Actividad\\Modificar Actividad";
            actividad = Actividad;

            this.Title = "Modificar Actividad";
            this.guardar.Text = "Guardar Cambios";

            this.nombre.Text = Actividad.nombre;
            this.tolerancia.Text = Actividad.tolerancia;

            // Eventos
            guardar.Clicked += Modificar_Clicked1;
            cancelar.Clicked += Cancelar_Clicked;

            // Contexto para los bindings
            this.BindingContext = this;
        }

        #region ========================== Modificar Actividad ==========================
        private async void Modificar_Clicked1(object sender, EventArgs e)
        {
            try
            {
                cambiarEstado(false);
                if (string.IsNullOrEmpty(nombre.Text))
                {
                    cambiarEstado(true);
                    await DisplayAlert("Modificar Actividad", "Debe rellenar el campo nombre", "Aceptar");
                    return;
                }
                object[,] variables = new object[,] {
                        { "idPlantillaActividad", actividad.idActividad} ,{ "nombre", nombre.Text}, { "tolerancia", tolerancia.Text }, { "idActividad", actividad.idPlantilla } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantillaActividad.asmx", "ModificarPlantillaActividad", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    cambiarEstado(true);
                    await DisplayAlert("Modificar Plantilla", Mensaje, "Aceptar");
                    App.ListarActividad.ActividadItems.Clear();
                    App.ListarActividad.CargarActividad();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await DisplayAlert("Modificar Actividad", "Error en el dispositivo o URL incorrecto: " + ex.Message, "Aceptar");
            }
        } 
        #endregion

        #region ========================== Cancelar ==========================
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        } 
        #endregion

        #region ========================== Guardar Actividad ==========================
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                cambiarEstado(false);
                if (string.IsNullOrEmpty(nombre.Text))
                {
                    cambiarEstado(true);
                    await DisplayAlert("Agregar Actividad", "Debe rellenar el nombre de la actividad.", "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(tolerancia.Text))
                    tolerancia.Text = "";
                object[,] variables = new object[,] { { "nombre", nombre.Text }, { "tolerancia", tolerancia.Text }, { "idActividad", plantilla.idPlantilla } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantillaActividad.asmx", "IngresarPlantillaActividad", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    cambiarEstado(true);
                    await App.Current.MainPage.DisplayAlert("Agregar Actividad", Mensaje, "Aceptar");
                    App.ListarActividad.ActividadItems.Clear();
                    App.ListarActividad.CargarActividad();
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                cambiarEstado(true);
                await DisplayAlert("Agregar Actividad", "Error en el dispositivo o URL incorrecto: " + ex.Message, "Aceptar");
            }
        }
        #endregion

        #region ============================== Cambiar Estado ==============================
        public void cambiarEstado(bool estado)
        {
            nombre.IsEnabled = estado;
            tolerancia.IsEnabled = estado;
            guardar.IsEnabled = estado;
            cancelar.IsEnabled = estado;
            if (estado == true) { IsRunning = false; }
            else { IsRunning = true; }
        }
        #endregion
    }
}
