using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Supervision
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarObraxCargo : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;
        WebService Servicio = new WebService();
        int IdUsuario;
        string Cargo;
        bool mostrarResponsable = false;
        bool mostrarAsistente = false;
        int nroElementos = App.nroElementos;
        int ultimoId = 100000;
        public ObservableCollection<ObraxCargoItem> ObraxCargoItems { get; set; }


        public bool isRefreshingObraxCargo { get; set; }
        public bool IsRefreshingObraxCargo
        {
            set
            {
                if (isRefreshingObraxCargo != value)
                {
                    isRefreshingObraxCargo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingObraxCargo"));
                }
            }
            get { return isRefreshingObraxCargo; }
        }

        public ICommand RefreshObraxCargoCommand { get; }

        #region ================================ Constructores ================================
        public ListarObraxCargo(int idUsuario, string cargo)
        {
            InitializeComponent();
            ObraxCargoItems = new ObservableCollection<ObraxCargoItem>();
            IdUsuario = idUsuario;
            Cargo = cargo;
            if (cargo == "Asistente") {
                CargarObrasAsistenteItems(nroElementos,ultimoId);
                mostrarAsistente = true;
            }
                
            if (cargo == "Responsable")
            {
                CargarObraResponsableItems(nroElementos,ultimoId);
                mostrarResponsable = true;
            }
               
          

            // Comandos
            RefreshObraxCargoCommand = new Command(() =>
            {
                ultimoId = 10000;
                ObraxCargoItems.Clear();
                if (cargo == "Asistente")
                    CargarObrasAsistenteItems(nroElementos,ultimoId);
                if (cargo == "Responsable")
                    CargarObraResponsableItems(nroElementos,ultimoId);
                IsRefreshingObraxCargo = false;
            });

            // Contexto para los bindings
            this.BindingContext = this;
        }

        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarObraxCargo = this;
        }

        private async void CargarObraResponsableItems(int elementos, int ultimo)
        {
            try
            {
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idResponsable", IdUsuario }, { "nroElementos", elementos }, { "ultimoId", ultimo } };
                dynamic obras = await servicio.MetodoPost("ServicioPropietarioObra.asmx", "MostrarResponsablexObra", variables);

                if (obras != null)
                {
                    if (obras.Count == 0) //si está vacío
                    {
                        await DisplayAlert("Mostrar Obra Responsable", "No Hay Obras a su cargo", "OK");
                    }
                    else
                    {
                        // listando las obras
                        foreach (var obra in obras)
                        {
                            ObraxCargoItems.Add(new ObraxCargoItem
                            {
                                nombre = obra.nombre,
                                idObra = obra.idObra,
                                codigoObra=obra.codigo,
                            });
                        }
                        // fin del listado
                    }
                }
                else
                {
                    await DisplayAlert("Mostrar Obra Responsable", "A ocurrido un error al listar las obras para este usuario", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async void CargarObrasAsistenteItems(int elementos,int ultimo)
        {
            try
            {
                
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idUsuario", IdUsuario }, { "nroElementos", elementos }, { "ultimoId", ultimo } };
                dynamic obras = await servicio.MetodoPost("ServicioUsuario.asmx", "MostrarObrasSupervision", variables);

                if (obras != null)
                {
                    if (obras.Count == 0) //si está vacío
                    {
                        await DisplayAlert("Mostrar Obra Responsable", "No Hay Obras a su cargo", "OK");
                    }
                    else
                    {
                        // listando las obras
                        foreach (var obra in obras)
                        {
                            ObraxCargoItems.Add(new ObraxCargoItem
                            {
                                nombre = obra.nombre,
                                idObra = obra.idObra,
                                codigoObra=obra.codigo
                            });
                        }
                        // fin del listado
                    }
                }
                else
                {
                    await DisplayAlert("Mostrar Obra Responsable", "A ocurrido un error al listar las obras para este usuario", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            ultimoId = ObraxCargoItems[ObraxCargoItems.Count - 1].idObra;
        }


        #region ================================ Scroll Infinito ================================
        
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = listViewObraxCargo.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {

            }
        }
        #endregion


    }
}
