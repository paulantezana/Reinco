using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Supervisar : ContentPage, INotifyPropertyChanged
    {


        #region +---- Atributos ----+
        public VentanaMensaje mensaje;
        private bool isRefreshingSupervisar { get; set; }
        #endregion




        #region +---- Services ----+
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        #endregion




        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion




        public bool IsRefreshingSupervisar
        {
            set
            {
                if (isRefreshingSupervisar != value)
                {
                    isRefreshingSupervisar = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingSupervisar"));
                }
            }
            get
            {
                return isRefreshingSupervisar;
            }
        }



        public ObservableCollection<SupervisarActividadItem> SupervisarActividadItems { get; set; }


        public Supervisar()
        {
            InitializeComponent();
            SupervisarActividadItems = new ObservableCollection<SupervisarActividadItem>();
            CargarSupervisarActividadItem();
            this.BindingContext = this;
        }

        private async void CargarSupervisarActividadItem()
        {
            try
            {
                for (int i = 1; i < 15; i++)
                {
                    SupervisarActividadItems.Add(new SupervisarActividadItem
                    {
                        item = i.ToString(),
                        actividad = "Remitos de mixer Completo (Texto Largo)",
                        observacionLevantada = true,
                        aprobacion = true,
                        anotacionAdicinal = "Img Tuberia  30 metros flexible",
                    });
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Error", ex.Message);
            }
        }
    }
}
