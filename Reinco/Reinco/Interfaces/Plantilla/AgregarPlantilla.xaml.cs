using Newtonsoft.Json;
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
    public partial class AgregarPlantilla : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;
        WebService Servicio = new WebService();
        int IdPlantilla;

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

        #region ========================= Constructores =========================
        // constructor Crear Plantilla
        public AgregarPlantilla()
        {
            InitializeComponent();
            directorio.Text = App.directorio + "/Plantilla/Crear Plantilla";

            // Eventos
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;

            // Contexto para los bindings
            this.BindingContext = this;
        }

        // construcor modificar plantilla
        public AgregarPlantilla(int idPlantilla, string codigo, string nombre)
        {
            InitializeComponent();
            directorio.Text = App.directorio + "/Plantilla/Modificar Plantilla";

            // Preparando interfas Modificar
            this.Title = "Modificar Plantilla";
            guardar.Text = "Guardar Cambios";
            IdPlantilla = idPlantilla;
            codPlantilla.Text = codigo;
            nombrePlantilla.Text = nombre;

            // Eventos
            guardar.Clicked += Modificar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;

            // Contexto para los bindings
            this.BindingContext = this;
        }
        #endregion

        #region ========================= Cancelar =========================
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        #endregion

        #region ========================= Modificar Plantilla =========================
        private async void Modificar_Clicked(object sender, EventArgs e)
        {
            try
            {
                cambiarEstado(false);
                if (string.IsNullOrEmpty(codPlantilla.Text) || string.IsNullOrEmpty(nombrePlantilla.Text))
                {
                    await DisplayAlert("Modificar plantilla", "Debe rellenar todos los campos.", "Aceptar");
                    cambiarEstado(true);
                    return;
                }
                object[,] variables = new object[,] { { "idPlantilla", IdPlantilla }, { "codigo", codPlantilla.Text }, { "nombre", nombrePlantilla.Text } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantilla.asmx", "ModificarPlantilla", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await DisplayAlert("Modificar Plantilla", Mensaje, "Aceptar");
                    App.ListarPlantilla.PlantillaItems.Clear();
                    App.ListarPlantilla.CargarPlantilla();
                    cambiarEstado(true);
                    await Navigation.PopAsync();
                    return;
                }
                await Navigation.PopAsync();
                return;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Modificar Plantilla", "Error en el dispositivo o URL incorrecto: " + ex.Message, "Aceptar");
                cambiarEstado(true);
            }
        }
        #endregion

        #region ========================= Guardar Plantilla =========================
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                cambiarEstado(false);
                if (string.IsNullOrEmpty(codPlantilla.Text) || string.IsNullOrEmpty(nombrePlantilla.Text))
                {
                    await DisplayAlert("Agregar plantilla", "Debe rellenar todos los campos.", "Aceptar");
                    cambiarEstado(true);
                    return;
                }
                object[,] variables = new object[,] { { "codigo", codPlantilla.Text }, { "nombre", nombrePlantilla.Text } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantilla.asmx", "IngresarPlantilla", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    guardar.IsEnabled = false;
                    await DisplayAlert("Agregar Plantilla", Mensaje, "Aceptar");
                    App.ListarPlantilla.PlantillaItems.Clear();
                    App.ListarPlantilla.CargarPlantilla();
                    cambiarEstado(true);
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Agregar Plantilla", "Verifique su conexión a internet. Si el problema persiste, contáctese con el administrador", "Aceptar");
                cambiarEstado(true);
            }
        }
        #endregion

        #region ============================== Cambiar Estado ==============================
        public void cambiarEstado(bool estado)
        {
            codPlantilla.IsEnabled = estado;
            nombrePlantilla.IsEnabled = estado;
            guardar.IsEnabled = estado;
            cancelar.IsEnabled = estado;
            if (estado == true) { IsRunning = false; }
            else { IsRunning = true; }
        }
        #endregion
    }
}
