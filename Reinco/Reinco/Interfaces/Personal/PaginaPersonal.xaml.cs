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
            var result = await client.GetAsync("http://192.168.1.37:80/ServicioUsuario.asmx/MostrarUsuarios");
            //recoge los datos json y los almacena en la variable resultado
            var resultado = await result.Content.ReadAsStringAsync();
            //si todo es correcto, muestra la pagina que el usuario debe ver
            dynamic array = JsonConvert.DeserializeObject(resultado);

            foreach (var item in array)
            {
                personalItem.Add(new PersonalItem
                {
                    fotoPerfil = "icon.png",
                    nombre = item.nombres.ToString(),
                    cargo1 = "gernte",
                    cargo1Tareas = "(3)",
                    cargo2 = "Supervisor",
                    cargo2Tareas = "(1)"
                });
            }
        }
        #endregion
        private void AgregarPersonal_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPersonal());
        }
    }
}
