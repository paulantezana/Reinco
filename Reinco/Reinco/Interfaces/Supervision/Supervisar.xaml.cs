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
        int IdSupervision;
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
        public Supervisar(int idSupervision)
        {
            InitializeComponent();
            IdSupervision = idSupervision;
            SupervisarActividadItems = new ObservableCollection<SupervisarActividadItem>();
            CargarSupervisarActividadItem();
            this.BindingContext = this;
        }
        private async void CargarSupervisarActividadItem()
        {
            byte x = 01;
            try
            {
                //object[,] variables = new object[,] { { "IdSupervision", IdSupervision } };
                //dynamic obras = await Servicio.MetodoGet("ServicioSupervision.asmx", "ActividadesxSupervision", variables);
                //foreach (var item in obras)
                //{

                //    SupervisarActividadItems.Add(new SupervisarActividadItem
                //    {
                //        item =x++.ToString(),
                //        actividad =item.nombre,
                //        tolerancia=item.tolerancia_maxima,
                //        observacionLevantada = true,
                //        aprobacion = true,
                //        anotacionAdicinal = "",
                //    });
                //}

                // Iteracion Solo Para Hacer Pruebas sin Web Service
                for (int i = 0; i < 15; i++)
                {
                    SupervisarActividadItems.Add(new SupervisarActividadItem
                    {
                        item = x++.ToString(),
                        actividad = "Nombre de la supervision",
                        tolerancia = "Tolerancia",
                        observacionLevantada = true,
                        aprobacion = true,
                        anotacionAdicinal = "Anotacion adicional",
                    });
                }
                // Fin Iteracion De solo Pruebas
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
           
        }
    }
}
