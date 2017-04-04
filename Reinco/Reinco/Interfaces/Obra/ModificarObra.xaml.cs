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

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModificarObra : ContentPage
    {
        int IdObra;
        int IdPropietario;
        int IdResponsabe;
        int IdPropietarioObra;

        WebService Servicio = new WebService();
        string Mensaje;
        public VentanaMensaje mensaje;


        public ObservableCollection<PropietarioItem> propietarioItems { get; set; }
        public ObservableCollection<PersonalItem> personalItems { get; set; }


        public ModificarObra(int idObra, string Codigo, string Nombre, int idPropietario, int idResponsable, int idPropietarioObra)
        {
            // Preparando la UI(Interfas de usuario) MODIFICAR OBRA
            InitializeComponent();
            this.Title = Nombre; // nombre de la pagina
            nombre.Text = Nombre; // Lenando el campo Nombre Obra
            codigo.Text = Codigo; // llenando el campo Codigo Obra

            IdObra = Convert.ToInt16(idObra);
            IdPropietario = idPropietario;
            IdResponsabe = idResponsable;
            IdPropietarioObra = idPropietarioObra;

            asignarPropietario.Title = "Asigne un nuevo propietario"; // Titulo POP UPS Propietario
            asignarResponsable.Title = "Asigne un nuevo responsable"; // Titulo POP UPS Responsable

            // Colecciones
            propietarioItems = new ObservableCollection<PropietarioItem>();
            personalItems = new ObservableCollection<PersonalItem>();
            CargarPropietarioItem();
            CargarPersonalItem();
            this.BindingContext = this;
        }

        private void CargarPropietarioItem()
        {
            throw new NotImplementedException();
        }

        private void CargarPersonalItem()
        {
            throw new NotImplementedException();
        }
    }
}
