using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaSupervision : ContentPage
    {
        public ObservableCollection<ObraSupervisarItem> obraSupervisarItem { get; set; }
        public PaginaSupervision()
        {
            InitializeComponent();
            obraSupervisarItem = new ObservableCollection<ObraSupervisarItem>();
            CargarObraSupervisarItem();
            obraSupervisarListView.ItemsSource = obraSupervisarItem;

            continuar.Clicked += Continuar_Clicked;
        }

        private void CargarObraSupervisarItem()
        {
            for (int i = 1; i < 11; i++)
            {
                obraSupervisarItem.Add(new ObraSupervisarItem
                {
                    item = Convert.ToString(i),
                    actividad = "Previo: Condicion Bomba",
                    aprobacionSi = true,
                    aprobacionNo = false,
                    observacionLevantada = false
                });
            }
        }

        private void Continuar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotasSupervision());
        }
    }
}
