using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Propietario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarPropietario : ContentPage
    {
        VentanaMensaje dialogService;
        private int IdPropietario;
        WebService Servicio = new WebService();
        string Mensaje;
        public VentanaMensaje mensaje;
        // ===================== Constructor Para Crear Propietario ===================== //
        public AgregarPropietario()
        {
            InitializeComponent();
            guardar.Clicked += Guardar_Clicked;
            dialogService = new VentanaMensaje();
            // eventos
            cancelar.Clicked += Cancelar_Clicked;
        }
        #region==================agregar propietario=====================
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(nombrePropietario.Text))
                {
                    await dialogService.MostrarMensaje("Agregar propietario", "debe rellenar todos los campos");
                    return;
                }
                
                    object[,] variables = new object[,] { { "propietario", nombrePropietario.Text } };
                    dynamic result = await Servicio.MetodoGetString("ServicioPropietario.asmx", "IngresarPropietario", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Agregar Propietario", Mensaje, "OK");
                        return;
                    }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Propietario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }

        }
        #endregion
        // ===================== Constructor Para Actualizar O Cambiar Propietario ===================== //
        public AgregarPropietario(object idPropietario)
        {
            InitializeComponent();
            this.IdPropietario =Convert.ToInt16( idPropietario);
            guardar.Text = "Guardar Cambios";
            guardar.Clicked += ModificarPropietario_Clicked1;
            cancelar.Clicked += Cancelar_Clicked;
        }

        private async void ModificarPropietario_Clicked1(object sender, EventArgs e)
        {
            try {
                if (string.IsNullOrEmpty(nombrePropietario.Text))
                {
                    await dialogService.MostrarMensaje("Modificar propietario", "Debe rellenar todos los campos.");
                    return;
                }
                object[,] variables = new object[,] { { "idPropietario", IdPropietario }, { "nombre", nombrePropietario.Text } };
                dynamic result = await Servicio.MetodoGetString("ServicioPropietario.asmx", "ModificarPropietario", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Modificar Propietario", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Modificar Propietario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }

        // Cacelar ============
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
