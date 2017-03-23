using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.ModelViews
{
    public class MainViewModel
    {

        #region Properties
        public ObservableCollection<ObraSupervisarItem> plantillaItem { get; set; }
        public ObservableCollection<PersonalItem> personalItem { get; set; }
        public ObservableCollection<PropietarioItem> propietarioItem { get; set; }
        public ObservableCollection<ObraItem> obraItem { get; set; }
        public ObservableCollection<TareaUsuarioItems> tareaUsuarioItems { get; set; }
        public ObservableCollection<PlantillaLista> plantillaLista { get; set; }
        public ObservableCollection<ActividadItems> actividadItems { get; set; }
        #endregion

        public LoginViewModel nuevoLogin { get; set; }

        #region Constructor
        public MainViewModel()
        {
            plantillaItem = new ObservableCollection<ObraSupervisarItem>();
            personalItem = new ObservableCollection<PersonalItem>();
            propietarioItem = new ObservableCollection<PropietarioItem>();
            obraItem = new ObservableCollection<ObraItem>();
            tareaUsuarioItems = new ObservableCollection<TareaUsuarioItems>();
            plantillaLista = new ObservableCollection<PlantillaLista>();
            actividadItems = new ObservableCollection<ActividadItems>();

            nuevoLogin = new LoginViewModel();

            CargarPlantillaItem();
            CargarPersonalItem();
            CargarPropietarioItem();
            //CargarObraItem();
            CargarTareaUsuarioItems();
            CargarPlantillaLista();
            CargarActividadItems();
        }

        #endregion

        #region Metodos
        private void CargarPlantillaItem()
        {
            for (int i = 1; i < 11; i++)
            {
                plantillaItem.Add(new ObraSupervisarItem
                {
                    item = Convert.ToString(i),
                    actividad = "Previo: Condicion Bomba",
                    aprobacionSi = true,
                    aprobacionNo = false,
                    observacionLevantada = false
                });
            }
        }
        private void CargarPersonalItem()
        {
            for (int i = 0; i < 10; i++)
            {
                personalItem.Add(new PersonalItem
                {
                    fotoPerfil = "icon.png",
                    nombre = "Juan",
                    cargo1 = "gernte",
                    cargo1Tareas = "(3)",
                    cargo2 = "Supervisor",
                    cargo2Tareas = "(1)"
                });
            }
        }
        private void CargarPropietarioItem()
        {
            for (int i = 0; i < 20; i++)
            {
                propietarioItem.Add(new PropietarioItem
                {
                    fotoPerfil = "icon.png",
                    nombre = "Perico"
                });
            }
        }

        

        private void CargarTareaUsuarioItems()
        {
            for (int i = 0; i < 50; i++)
            {
                tareaUsuarioItems.Add(new TareaUsuarioItems
                {
                    titulo = "Titulo De La Tarea",
                    descripcion = "Descripcion De La Tarea",
                    numeroTarea = "67"
                });
            }
        }
        private void CargarPlantillaLista()
        {
            for (int i = 0; i < 30; i++)
            {
                plantillaLista.Add(new PlantillaLista
                {
                    codigo = "F08",
                    nuemroItems = "Items (10)",
                    agregarActividad = "las"
                });
            }
        }
        private void CargarActividadItems()
        {
            for (int i = 0; i < 30; i++)
            {
                actividadItems.Add(new ActividadItems
                {
                    numeroActividad = Convert.ToString(i),
                    titulo = "Titulo De La Actividad"
                });
            }
        }
        #endregion

    }
}
