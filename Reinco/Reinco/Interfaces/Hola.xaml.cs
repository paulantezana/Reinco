using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reinco.Entidades;
using Reinco.Recursos;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;

namespace Reinco.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Hola : ContentPage
    {
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        public Hola()
        {
            InitializeComponent();
            guardar.Clicked += Guardar_Clicked;
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            object[,] variables = new object[,] { { "IdSupervision", 1 } };
            dynamic obras = await Servicio.MetodoGet("ServicioSupervision.asmx", "ActividadesxSupervision", variables);
            foreach (var item in obras)
            {
                
                // await Task.Delay(100);
            }
        }
    }
}
