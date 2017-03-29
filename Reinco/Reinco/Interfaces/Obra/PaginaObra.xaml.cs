using Newtonsoft.Json;
using Reinco.Gestores;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaObra : ContentPage
    {
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;
        public ObservableCollection<ObraItem> obraItem { get; set; }
        public PaginaObra()
        {
            InitializeComponent();
            obraItem = new ObservableCollection<ObraItem>();
            CargarObraItem();
            obrasListView.ItemsSource = obraItem;

            agregarObra.Clicked += AgregarObra_Clicked;
        }
        #region==================cargar obras======================================
        public async void CargarObraItem()
        {
            try
            {
                dynamic result = await Servicio.MetodoGet("ServicioObra.asmx", "MostrarObras");
                foreach (var item in result)
                {
                    obraItem.Add(new ObraItem
                    {
                        idObra = item.idObra,
                        nombre = item.nombre,
                        codigo = item.codigo,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion
        // ===================// Navegar A la página AgregarObra.xaml //====================//
        private void AgregarObra_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarObra());
        }

        #region=======================eliminar obra====================================
        public async void eliminar(object sender, EventArgs e)
        {
            try { 
                var idObra = ((MenuItem)sender).CommandParameter;
                int IdObra = Convert.ToInt16(idObra);
                bool respuesta= await DisplayAlert("Eliminar", "Eliminar idObra = " + idObra, "Aceptar","Cancelar");
                object[,] variables = new object[,] { { "idObra", IdObra } };
                dynamic result = await Servicio.MetodoGetString("ServicioObra.asmx", "EliminarObra", variables);
                Mensaje = Convert.ToString(result);
                if (result!=null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Obra", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Obra", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
            
        }
        #endregion
        #region ===================// Modificar Obra CRUD //====================
        public void actualizar(object sender, EventArgs e)
        {
            
            var idObra = ((MenuItem)sender).CommandParameter;
            //  var elementos = obrasListView.
            Navigation.PushAsync(new AgregarObra(idObra));
        }

        #endregion

    }
}
