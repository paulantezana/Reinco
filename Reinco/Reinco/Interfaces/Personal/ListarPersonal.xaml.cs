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
                    //Determino el color según el cargo
                    string color = "";
                    bool visible_asignarPlantilla = false;
                    bool visible_asignarObra = false;

                    if (Convert.ToInt32(item.idCargo) == 1)//administrador
                        color = "#FF7777";
                    else if (Convert.ToInt32(item.idCargo) == 2)//responable
                    {
                        color = "#FF2222";
                        visible_asignarObra = true;
                    }
                    else if (Convert.ToInt32(item.idCargo) == 2)//supervisor
                    {
                        color = "#FF4444";
                        visible_asignarPlantilla = true;
                    }
                    else
                        color = "#FF6666";

                    this.personalItem.Add(new PersonalItem
                    {
                        //Inicializo todo aunque no lo vaya a usar porque luego puedo necesitar todo para editar
                        fotoPerfil = "icon.png",
                        idUsuario = item.idUsuario,
                        dni = item.dni,
                        nombresApellidos = item.nombresApellidos,
                        usuario = item.usuario,
                        contrasena = item.contrasena,
                        correo = item.correo,
                        celular = item.celular,
                        cip = item.cip,
                        idCargo_Usuario = Convert.ToInt32(item.idCargo_Usuario),
                        idCargo = Convert.ToInt32(item.idCargo),
                        colorCargo = color,
                        visible_asignarPlantilla = visible_asignarPlantilla,
                        visible_asignarObra = visible_asignarObra
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
