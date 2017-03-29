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
            //asignados
            // Eventos
            cancelar.Clicked += Cancelar_Clicked;
            guardar.Clicked += Guardar_Clicked;
        }
        #region Metodos Para Listar Personal Y Propietarios
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

        private async void CargarPropietarioItem()
        {
            try
            {
                dynamic result = await Servicio.MetodoGet("ServicioPropietario.asmx", "MostrarPropietarios");
                foreach (var item in result)
                {
                    propietarioItem.Add(new PropietarioItem
                    {
                        idPropietario = item.idPropietario,
                        nombre = item.nombre,
                        fotoPerfil = "icon.png",
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
            try
            {
                if (asignarPropietario.SelectedValue == null && asignarResponsable.SelectedValue == null)
                {
                    #region================ingresar solo obra=============================
                    if (string.IsNullOrEmpty(codigo.Text) || string.IsNullOrEmpty(nombre.Text))
                    {
                        await DisplayAlert("Agregar Obra", "Debe rellenar todos los campos.", "OK");
                        return;
                    }
                    object[,] variables = new object[,] { { "idObra", IdObra }, { "codigo", codigo.Text }, { "nombreObra", nombre.Text } };
                    dynamic result = await Servicio.MetodoGetString("ServicioObra.asmx", "IngresarObra", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Agregar Obra", Mensaje, "OK");
                        ListarObra paginaObra = new ListarObra();
                        paginaObra.CargarObraItem();
                        await Navigation.PopAsync();
                        return;
                    }

                    #endregion
                }
                #region===========ingresar con responsable y propietario=========
                else
                {
                    int idPropietario = Convert.ToInt16(asignarPropietario.SelectedValue);
                    int idUsuario = Convert.ToInt16(asignarResponsable.SelectedValue);
                    object[,] variables = new object[,] { { "codigoObra", codigo.Text }, { "nombreObra", nombre.Text },
                   { "idPropietario",  idPropietario }, { "idUsuarioResponsable", idUsuario} };
                    dynamic result = await Servicio.MetodoGetString("ServicioPropietarioObra.asmx", "IngresarPropietarioResponsabledEnObra", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Agregar Obra con Responsable y Propietario", Mensaje, "OK");
                        return;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Agregar Obra", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
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
        public AgregarObra(int IdObra, string Codigo, string Nombre)
        {
            InitializeComponent();
            nombre.Text = Nombre;
            codigo.Text = Codigo;
            this.Title = "MODIFICAR OBRA";
            guardar.Clicked += modificarObra;
            IdObra = Convert.ToInt16(IdObra);
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
