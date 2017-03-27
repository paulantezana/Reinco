using Newtonsoft.Json;
using Reinco.Gestores;
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
    public partial class PaginaPersonal : ContentPage
    {
        public ObservableCollection<PersonalItem> personalItem { get; set; }
        
        public PaginaPersonal()
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
            var client = new HttpClient();
            var result = await client.GetAsync("http://192.168.1.37:8080/ServicioUsuario.asmx/MostrarUsuarios");
            //recoge los datos json y los almacena en la variable resultado
            var resultado = await result.Content.ReadAsStringAsync();
            //si todo es correcto, muestra la pagina que el usuario debe ver
            dynamic array = JsonConvert.DeserializeObject(resultado);

            foreach (var item in array)
            {
                personalItem.Add(new PersonalItem
                {
                    fotoPerfil = "icon.png",
                    idUsuario = item.idUsuario,
                    nombres = item.nombres.ToString(),
                    cip = item.cip
                });
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
            var idUsuario = ((MenuItem)sender).CommandParameter;
            int IdUsuario = Convert.ToInt16(idUsuario);
            bool respuesta = await DisplayAlert("Eliminar", "Eliminar IdUsuario = " + IdUsuario, "Aceptar", "Cancelar");
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
