using Reinco.Gestores;
using Reinco.Recursos;
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
    public partial class CrearSupervision : ContentPage
    {
        public ObservableCollection<PersonalItem> personalItem { get; set; }
        public VentanaMensaje mensaje;
        public CrearSupervision()
        {
            InitializeComponent();
            personalItem = new ObservableCollection<PersonalItem>();
            mensaje = new VentanaMensaje();

            // cargar listas
            CargarPersonalItem();

        }



        #region // ================================= Cargando Personal ================================= //
        private async void CargarPersonalItem()
        {
            try
            {
                WebService servicio = new WebService();
                object[,] variables = new object[,] { };
                dynamic result = await servicio.MetodoGet("ServicioUsuario.asmx", "MostrarUsuarios", variables);
                if (result != null)
                {
                    if (result.Count == 0) //si está vacío
                    {
                        result = "No hay personal para mostrar";
                    }

                    foreach (var item in result)
                    {
                        personalItem.Add(new PersonalItem
                        {
                            idUsuario = item.idUsuario,
                            nombresApellidos = item.nombresApellidos,
                        });
                    }
                    asignarSupervisor.ItemsSource = personalItem;
                }
                else
                {
                    await mensaje.MostrarMensaje("Personal", "No se pudo cargar el personal correctamente");

                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Cargar Personal", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        } 
        #endregion



    }
}
