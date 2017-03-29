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

namespace Reinco.Interfaces.Personal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarPersonal : ContentPage
    {
        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;
        public ObservableCollection<PersonalItem> personalItem { get; set; }
        
        public ListarPersonal()
        {
            InitializeComponent();
            personalItem = new ObservableCollection<PersonalItem>();
            CargarPersonalItem();
            personalListView.ItemsSource = personalItem;
            agregarPersonal.Clicked += AgregarPersonal_Clicked;
        }
        #region==================cargar usuarios==============================
        private async void CargarPersonalItem()
        {
            try
            {
                dynamic result = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuarios");
                foreach (var item in result)
                {
                    personalItem.Add(new PersonalItem
                    {
                        fotoPerfil = "icon.png",
                        idUsuario = item.idUsuario,
                        nombresApellidos = item.nombresApellidos.ToString(),
                        cip = item.cip
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion
        private void AgregarPersonal_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPersonal());
        }

        #region=======================eliminar obra====================================
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {
                var idUsuario = ((MenuItem)sender).CommandParameter;
                int IdUsuario = Convert.ToInt16(idUsuario);
                bool respuesta = await DisplayAlert("Eliminar", "Eliminar IdUsuario = " + IdUsuario, "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idUsuario", IdUsuario } };
                dynamic result = await Servicio.MetodoGetString("ServicioUsuario.asmx", "EliminarUsuario", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Usuario", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Usuario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        #endregion
        #region ===================// Modificar Obra CRUD //====================
        public void actualizar(object sender, EventArgs e)
        {
            var idUsuario = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new AgregarPersonal(idUsuario));
        }
        #endregion

    }
}
