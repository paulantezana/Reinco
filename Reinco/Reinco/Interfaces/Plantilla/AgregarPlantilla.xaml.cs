using Newtonsoft.Json;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarPlantilla : ContentPage
    {
        int IdPlantilla;
        DialogService dialogService;

        // ===================== Constructor Para Crear Plantilla =================== //
        public AgregarPlantilla()
        {
            InitializeComponent();
            // servicios
            dialogService = new DialogService();

            // Eventos
            guardar.Clicked += Guardar_Clicked;
            cancelar.Clicked += Cancelar_Clicked;
        }

        
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        #region ===================== Agregar PLantillas =====================
        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(codPlantilla.Text) || string.IsNullOrEmpty(nombrePlantilla.Text) )
            {
                await dialogService.MostrarMensaje("Agregar plantilla", "Debe rellenar todos los campos.");
                return;
            }
            
                using (var cliente = new HttpClient())
                {
                    var result = await cliente.GetAsync("http://192.168.1.37:8080/ServicioPlantilla.asmx/IngresarPlantilla?codigo=" + codPlantilla.Text + "&nombre=" + nombrePlantilla.Text);
                    var json = await result.Content.ReadAsStringAsync();
                    string mensaje = Convert.ToString(json);
                   
                    if (result.IsSuccessStatusCode)
                        {
                            await App.Current.MainPage.DisplayAlert("Agregar Plantilla", mensaje, "OK");
                            return;
                        }
                    }
            
        }
        #endregion
        // ===================== Constructor Para Actualizar O Cambiar Plantilla ===================== //
        public AgregarPlantilla(object idPlantilla)
        {
            InitializeComponent();
            IdPlantilla = Convert.ToInt16(idPlantilla);
            guardar.Clicked += Guardar_Clicked1;
            cancelar.Clicked += Cancelar_Clicked1;
        }
        #region==================modificar plantilla================================
        private async  void Guardar_Clicked1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(codPlantilla.Text) || string.IsNullOrEmpty(nombrePlantilla.Text))
            {
                await dialogService.MostrarMensaje("Modificar plantilla", "Debe rellenar todos los campos.");
                return;
            }

            using (var cliente = new HttpClient())
            {
                var result = await cliente.GetAsync("http://192.168.1.37:8080/ServicioPlantilla.asmx/ModificarPlantilla?idPlantilla=" + IdPlantilla+"&codigo="+codPlantilla.Text + "&nombre=" + nombrePlantilla.Text);
                var json = await result.Content.ReadAsStringAsync();
                string mensaje = Convert.ToString(json);

                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Modificar Plantilla", mensaje, "OK");
                    return;
                }
            }
        }
        #endregion
        #region ===================== Agregar PLantillas =====================

        #endregion
        private void Cancelar_Clicked1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        // ===================== END
    }
}
