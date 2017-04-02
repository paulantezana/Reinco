using Reinco.Entidades;
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
    public partial class AsignarPlantilla : ContentPage
    {
        int IdPlantillaPropietarioObra;
        WebService Servicio = new WebService();
        string Mensaje;
        public ObservableCollection<PlantillaItem> plantillaLista { get; set; }
        //public ObservableCollection<PersonalItem> personalItem { get; set; }
        public AsignarPlantilla()
        {
            InitializeComponent();
            this.Title = "Plantillas disponibles";

            plantillaLista = new ObservableCollection<PlantillaItem>();
            //personalItem = new ObservableCollection<PersonalItem>();
            guardar.Clicked += Guardar_Clicked;
            //// cargar listas
            CargarPlantillaLista();
            //CargarPersonalItem();

            //// listar
            asignarPlantilla.ItemsSource = plantillaLista;
            //asignarSupervisor.ItemsSource = personalItem;
            // -------------
        }
        public AsignarPlantilla(object idPlantillaPropietarioObra)
        {
            InitializeComponent();
            this.Title = "Plantillas disponibles";
            IdPlantillaPropietarioObra = Convert.ToInt16(idPlantillaPropietarioObra);
            plantillaLista = new ObservableCollection<PlantillaItem>();
            //personalItem = new ObservableCollection<PersonalItem>();
            guardar.Clicked += Guardar_Clicked;
            //// cargar listas
            CargarPlantillaLista();
            //CargarPersonalItem();

            //// listar
            asignarPlantilla.ItemsSource = plantillaLista;
            //asignarSupervisor.ItemsSource = personalItem;
            // -------------
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            #region================ingresar solo obra=============================
            if (asignarPlantilla.SelectedValue==null)
            {
                await DisplayAlert("Asignar Plantilla","Debe Asignar alguna plantills", "Aceptar");
                return;
            }
            object[,] variables = new object[,] { { "idPlantilla", asignarPlantilla.SelectedValue }, { "idPropietarioObra", IdPlantillaPropietarioObra } };
            dynamic result = await Servicio.MetodoGetString("ServicioPlantillaPropietarioObra.asmx", "IngresarPlantillaPropietarioObra", variables);
            Mensaje = Convert.ToString(result);
            if (result != null)
            {
                await DisplayAlert("Asignar Plantilla", Mensaje, "Aceptar");

                // Refrescando la lista
               // App.ListarObra.ObraItems.Clear();
                App.ListarObraPlantilla.ObraPlantillaItems.Clear();
                App.ListarObraPlantilla.CargarPlantillaObra();
                await Navigation.PopAsync();
                return;
            }

            #endregion
        }

        #region Listar plantilla y supervisor
        //private void CargarPersonalItem()
        //{
        //    try
        //    {
        //        for (int i = 0; i < 12; i++)
        //        {
        //            personalItem.Add(new PersonalItem
        //            {
        //                idUsuario = i,
        //                nombres = "Nombre personal",
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayAlert("Error", ex.Message, "Aceptar");
        //    }
        //}
        #endregion

        private async void CargarPlantillaLista()
        {
            try
            {
                dynamic plantillas = await Servicio.MetodoGet("ServicioPlantilla.asmx", "MostrarPlantillas");
                foreach (var plantilla in plantillas)
                {
                    plantillaLista.Add(new PlantillaItem
                    {
                        idPlantilla = plantilla.idPlantilla,
                        codigo = plantilla.codigo,
                        nombre = plantilla.nombre,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        //#endregion
    }
}
