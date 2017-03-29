using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AsignarPlantilla : ContentPage
    {
        //public ObservableCollection<PlantillaLista> plantillaLista { get; set; }
        //public ObservableCollection<PersonalItem> personalItem { get; set; }
        public AsignarPlantilla(int IdObra, string Nombre)
        {
            InitializeComponent();
            this.Title = Nombre;

            //plantillaLista = new ObservableCollection<PlantillaLista>();
            //personalItem = new ObservableCollection<PersonalItem>();

            //// cargar listas
            //CargarPlantillaLista();
            //CargarPersonalItem();

            //// listar
            //asignarPlantilla.ItemsSource = plantillaLista;
            //asignarSupervisor.ItemsSource = personalItem;
            // -------------
        }

        //#region Listar plantilla y supervisor
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

        //private void CargarPlantillaLista()
        //{
        //    try
        //    {
        //        for (int i = 0; i < 15; i++)
        //        {
        //            plantillaLista.Add(new PlantillaLista
        //            {
        //                idPlantilla = i,
        //                nombre = "Nombre Plantilla",
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayAlert("Error", ex.Message, "Aceptar");
        //    }
        //} 
        //#endregion
    }
}
