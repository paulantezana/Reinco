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
            CargarTareaUsuarioItems();

            // renderisando las listas
            asignarPropietario.ItemsSource = propietarioItem;
            asignarResponsable.ItemsSource = personalItem;

            // Eventos
            cancelar.Clicked += Cancelar_Clicked1;
            guardar.Clicked += Guardar_Clicked;
        }

        #region Metodos Para Listar Personal Y Propietarios
        private void CargarTareaUsuarioItems()
        {
            for (int i = 0; i < 15; i++)
            {
                personalItem.Add(new PersonalItem
                {
                    idUsuario = i,
                    nombres = "Nombre Usuario",
                });
            }
        }

        private void CargarPropietarioItem()
        {
            for (int i = 0; i < 15; i++)
            {
                propietarioItem.Add(new PropietarioItem
                {
                    idPropietario = i,
                    nombre = "Nombre Propietario",
                });
            }
        } 
        #endregion

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(codigo.Text) || string.IsNullOrEmpty(nombre.Text) )
            {
                await DisplayAlert("Agregar Obra", "Debe rellenar todos los campos.","OK");
                return;
            }
            
                using (var cliente = new HttpClient())
                {
                    var result = await cliente.GetAsync("http://192.168.1.37/ServicioObra.asmx/IngresarObra?codigo=" + codigo.Text + "&nombreObra=" + nombre.Text);
                    if (result.IsSuccessStatusCode)
                    {
                        await App.Current.MainPage.DisplayAlert("Obra Agregada", "Obra agregada satisfactoriamente.", "OK");
                        return;
                    }
                }
            
        }

        #region Navegacion para el voton cancelar
        // boton Cancelar de crear obra
        private void Cancelar_Clicked1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        // boton cancelar de modificar y eliminar obra
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
        private async void modificarObra(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(codigo.Text) || string.IsNullOrEmpty(nombre.Text))
            {
                await DisplayAlert("Agregar Obra", "Debe rellenar todos los campos.", "OK");
                return;
            }

            using (var cliente = new HttpClient())
            {
                var result = await cliente.GetAsync("http://192.168.1.37/ServicioObra.asmx/ModificarObra?idObra="
                    + IdObra + "&codigo=" + codigo.Text+ "&nombreObra=" + nombre.Text);
                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Obra Modificada", "Obra modificada satisfactoriamente.", "OK");
                    return;
                }
            }

        }
    }
}
