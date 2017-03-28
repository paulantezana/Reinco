using Newtonsoft.Json;
using Reinco.Gestores;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarObra : ContentPage
    {
        int IdObra;
        WebService Servicio = new WebService();
        string Mensaje;
        public VentanaMensaje mensaje;
        public ObservableCollection<PropietarioItem> propietarioItem { get; set; }
        public ObservableCollection<PersonalItem> personalItem { get; set; }
        // ============================ Constructor para crear obra ============================//
        public AgregarObra()
        {
            InitializeComponent();
            // =====
            propietarioItem = new ObservableCollection<PropietarioItem>();
            personalItem = new ObservableCollection<PersonalItem>();

            // cargando la listas
            CargarPropietarioItem();
            CargarPersonalItem();

            // renderisando las listas
            asignarPropietario.ItemsSource = propietarioItem;
            asignarResponsable.ItemsSource = personalItem;

            // Eventos
            cancelar.Clicked += Cancelar_Clicked;
            guardar.Clicked += Guardar_Clicked;
        }
        #region Metodos Para Listar Personal Y Propietarios
        private async void CargarPersonalItem()
        {
            try
            {
                HttpClient client = new HttpClient();
                var result = await client.GetAsync("http://192.168.1.37:8080/ServicioUsuario.asmx/MostrarUsuarios");
                //recoge los datos json y los almacena en la variable resultado
                var resultado = await result.Content.ReadAsStringAsync();
                //si todo es correcto, muestra la pagina que el usuario debe ver
                dynamic array = JsonConvert.DeserializeObject(resultado);

                foreach (var item in array)
                {
                    personalItem.Add(new PersonalItem
                    {
                        idUsuario = item.idUsuario,
                        nombres = item.nombres,
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        private async void CargarPropietarioItem()
        {
            try
            {
                var client = new HttpClient();
                var result = await client.GetAsync("http://192.168.1.37:8080/ServicioPropietario.asmx/MostrarPropietarios");
                //recoge los datos json y los almacena en la variable resultado
                var resultado = await result.Content.ReadAsStringAsync();
                //si todo es correcto, muestra la pagina que el usuario debe ver
                dynamic array = JsonConvert.DeserializeObject(resultado);

                foreach (var item in array)
                {
                    propietarioItem.Add(new PropietarioItem
                    {
                        idPropietario = item.idPropietario,
                        nombre = item.nombre,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        } 
        #endregion

        private async void Guardar_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(codigo.Text) || string.IsNullOrEmpty(nombre.Text))
            {
                await DisplayAlert("Agregar Obra", "Debe rellenar todos los campos.", "OK");
                return;
            }
            //comentario
            object[,] variables = new object[,] { { "idObra", IdObra }, { "codigo", codigo.Text }, { "nombreObra", nombre.Text } };
            dynamic result = await Servicio.MetodoGetString("ServicioObra.asmx", "IngresarObra", variables);
            Mensaje = Convert.ToString(result);
            if (result != null)
            {
                await App.Current.MainPage.DisplayAlert("Agregar Obra", Mensaje, "OK");
                return;
            }

        }
        #region Navegacion para el boton cancelar

        // boton cancelar
        private void Cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        } 
        #endregion

        // ============== Constructor para para modificar o eliminar la  obra ===============//
        public AgregarObra(object idObra)
        {
            InitializeComponent();
            guardar.Clicked += modificarObra;
            IdObra = Convert.ToInt16(idObra);
        }
        #region==================modificar obra=================================
        private async void modificarObra(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(codigo.Text) || string.IsNullOrEmpty(nombre.Text))
                {
                await DisplayAlert("Modificar Obra", "Debe rellenar todos los campos.", "OK");
                return;
                 }
                object[,] variables = new object[,] { { "idObra", IdObra } , { "codigo", codigo.Text } , { "nombreObra", nombre.Text } };
                dynamic result = await Servicio.MetodoGetString("ServicioObra.asmx", "ModificarObra", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Modificar Obra", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Modificar Obra", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
            

        }
        #endregion
    }
}
