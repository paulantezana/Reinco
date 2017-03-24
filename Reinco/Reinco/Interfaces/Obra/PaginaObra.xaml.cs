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

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaObra : ContentPage
    {
        public ObservableCollection<ObraItem> obraItem { get; set; }
        public PaginaObra()
        {
            InitializeComponent();
            obraItem = new ObservableCollection<ObraItem>();
            CargarObraItem();
            obrasListView.ItemsSource = obraItem;
            agregarObra.Clicked += AgregarObra_Clicked;
        }

        public async void CargarObraItem()
        {
            var client = new HttpClient();
            var result = await client.GetAsync("http://192.168.1.37:80/ServicioObra.asmx/MostrarObras");
            //recoge los datos json y los almacena en la variable resultado
            var resultado = await result.Content.ReadAsStringAsync();
            //si todo es correcto, muestra la pagina que el usuario debe ver
            dynamic array = JsonConvert.DeserializeObject(resultado);

            foreach (var item in array)
            {
                obraItem.Add(new ObraItem
                {
                    titulo = item.nombre.ToString(),
                    responsable = "nombre del responsable",
                    platilla = "PLANTILLAS"
                });
            }

        }

        private void AgregarObra_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarObra());
        }
    }
}
