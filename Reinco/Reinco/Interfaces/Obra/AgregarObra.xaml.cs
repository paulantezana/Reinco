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
        int idPropietario;
        int idUsuario;
        int IdPropietarioObra;
        WebService Servicio = new WebService();
        string Mensaje;
        public VentanaMensaje mensaje;
        public ObservableCollection<PropietarioItem> propietarioItem { get; set; }
        public ObservableCollection<PersonalItem> personalItem { get; set; }

        #region +---- Constructores ----+
        public AgregarObra()
        {
            InitializeComponent();
            this.Title = "Crear Obra"; // nombre de la pagina

            propietarioItem = new ObservableCollection<PropietarioItem>();
            personalItem = new ObservableCollection<PersonalItem>();

            // cargando la listas
            CargarPropietarioItem();
            CargarPersonalItem();

            // Eventos Guardar Y Cancelar
            cancelar.Clicked += Cancelar_Clicked;
            guardar.Clicked += Guardar_Clicked;
        }

        public AgregarObra(int idObra, string Codigo, string Nombre)
        {
            InitializeComponent();
            this.Title = Nombre; // nombre de la pagina

            // llenando los campos
            nombre.Text = Nombre;
            codigo.Text = Codigo;
            IdObra = Convert.ToInt16(idObra);

            // Cambiando el texto del boton guardar
            guardar.Text = "Guardar Cambios";

            // Eventos Guardar Y Cancelar
            guardar.Clicked += modificarObra;
            cancelar.Clicked += Cancelar_Clicked;
        } 
        #endregion


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
                        fotoPerfil = "ic_profile.png",
                        idUsuario = item.idUsuario,
                        nombresApellidos = item.nombresApellidos.ToString(),
                        cip = item.cip
                    });
                }
                asignarResponsable.ItemsSource = personalItem; // pintando en la interfas de usuario
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
                        fotoPerfil = "ic_profile.png",
                    });
                }
                asignarPropietario.ItemsSource = propietarioItem; // Pintando en la interfas de usuario
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
                    object[,] variables = new object[,] { { "codigo", codigo.Text }, { "nombreObra", nombre.Text } };
                    dynamic result = await Servicio.MetodoGetString("ServicioObra.asmx", "IngresarObra", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Agregar Obra", Mensaje, "OK");
                        ListarObra paginaObra = new ListarObra();
                        paginaObra.CargarObraItems();
                        await Navigation.PopAsync();
                        return;
                    }

                    #endregion
                }
                #region===========ingresar con responsable y propietario=============
                else
                {
                    if (asignarPropietario.SelectedValue != null && asignarResponsable.SelectedValue!=null)
                    {
                         idPropietario = Convert.ToInt16(asignarPropietario.SelectedValue);
                         idUsuario = Convert.ToInt16(asignarResponsable.SelectedValue);
                         IngresarPropResponsable(idPropietario, idUsuario);
                    }
                    else {
                        if (asignarPropietario.SelectedValue == null && asignarResponsable.SelectedValue != null)
                        {
                            idUsuario= Convert.ToInt16(asignarResponsable.SelectedValue);
                            IngresarPropResponsable(0, idUsuario);
                        }
                        if (asignarPropietario.SelectedValue != null && asignarResponsable.SelectedValue == null)
                        {
                            idPropietario = Convert.ToInt16(asignarPropietario.SelectedValue);
                            IngresarPropResponsable(idPropietario, 0);
                        }
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

        // ============== Ingresar Propietario y Responsable  ===============//

        public async void IngresarPropResponsable(object idPropietario,object idUsuario)
        {
            object[,] variables = new object[,] { { "codigoObra", codigo.Text }, { "nombreObra", nombre.Text },
                           { "idPropietario",  idPropietario }, { "idUsuarioResponsable", idUsuario} };
            dynamic result = await Servicio.MetodoGetString("ServicioPropietarioObra.asmx", "IngresarPropietarioResponsableEnObra", variables);
            Mensaje = Convert.ToString(result);
            if (result != null)
            {
                await App.Current.MainPage.DisplayAlert("Agregar Obra con Responsable y Propietario", Mensaje, "OK");
                return;
            }
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
        #region===============Modificar Obra Propietario y Responsable==========================
        public async void ModificarPropietarioResponsableObra(object IdPropietario, object IdResponsable)
        {
            object[,] variables = new object[,] { { "codigoObra", codigo.Text }, { "nombreObra", nombre.Text },
                { "IdObra", IdObra },{ "IdPropietario", IdPropietario}, { "IdResponsable", IdResponsable},
                { "IdPropietarioObra", IdPropietarioObra}};
            dynamic result = await Servicio.MetodoGetString("ServicioObra.asmx", "ModificarObra", variables);
            Mensaje = Convert.ToString(result);
            if (result != null)
            {
                await App.Current.MainPage.DisplayAlert("Modificar Obra", Mensaje, "OK");
                return;
            }
        }
        #endregion
    }
}
