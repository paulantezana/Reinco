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

namespace Reinco.Interfaces.Propietario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarPropietario : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;
        WebService Servicio = new WebService();
        PropietarioItem propietario;
        int ultimoId = 100000;
        int nroElementos = App.nroElementos;
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

        public AgregarPropietario()
        {
            InitializeComponent();
            directorio.Text = App.directorio + "/Propietario/Agregar Propietario";

            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;

            // Contexto Para Los bindings
            this.BindingContext = this;
        }

        public AgregarPropietario(PropietarioItem Propietario)
        {
            // Preparando la interfas para modificar propietario
            InitializeComponent();
            propietario = Propietario;
            directorio.Text = App.directorio + "/Propietario/Modificar Propietario";
            this.Title = "MODIFICAR PROPIETARIO";
            guardar.Text = "Guardar Cambios";
            nombrePropietario.Text = Propietario.nombre;

            // Eventos
            cancelar.Clicked += Cancelar_Clicked;
            guardar.Clicked += Modificar_Clicked1;

            // Contexto Para Los bindings
            this.BindingContext = this;
        }
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        #region ============================ Modificar Propietario ============================
        private async void Modificar_Clicked1(object sender, EventArgs e)
        {
            try
            {
                cambiarEstado(false);
                if (string.IsNullOrEmpty(nombrePropietario.Text))
                {
                    await DisplayAlert("Modificar propietario", "Debe rellenar todos los campos.", "Aceptar");
                    cambiarEstado(true);
                    return;
                }
                object[,] variables = new object[,] { { "idPropietario", propietario.idPropietario }, { "nombre", nombrePropietario.Text } };
                dynamic result = await Servicio.MetodoGetString("ServicioPropietario.asmx", "ModificarPropietario", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Modificar Propietario", Mensaje, "OK");
                    App.ListarPropietarios.PropietarioItems.Clear();
                    App.ListarPropietarios.CargarPropietarioItem(nroElementos, ultimoId);
                    cambiarEstado(true);
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error en el dispositivo o URL incorrecto: " + ex.Message, "Aceptar");
                cambiarEstado(true);
            }
        } 
        #endregion

        #region  ============================ Guardar Nuevo Propietario ============================
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                cambiarEstado(false);
                if (string.IsNullOrEmpty(nombrePropietario.Text))
                {
                    await DisplayAlert("Agregar propietario", "debe rellenar todos los campos", "Aceptar");
                    cambiarEstado(true);
                    return;
                }

                object[,] variables = new object[,] { { "propietario", nombrePropietario.Text } };
                dynamic result = await Servicio.MetodoGetString("ServicioPropietario.asmx", "IngresarPropietario", variables);
                string Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    guardar.IsEnabled = false;
                    await App.Current.MainPage.DisplayAlert("Agregar Propietario", Mensaje, "OK");
                    App.ListarPropietarios.PropietarioItems.Clear();
                    App.ListarPropietarios.CargarPropietarioItem(nroElementos,ultimoId);
                    cambiarEstado(true);
                    await Navigation.PopAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error en el dispositivo o URL incorrecto: " + ex.Message, "Aceptar");
                cambiarEstado(true);
            }
        } 
        #endregion

        #region ============================== Cambiar Estado ==============================
        public void cambiarEstado(bool estado)
        {
            nombrePropietario.IsEnabled = estado;
            guardar.IsEnabled = estado;
            cancelar.IsEnabled = estado;
            if (estado == true) { IsRunning = false; }
            else { IsRunning = true; }
        }
        #endregion

    }
}
