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
        public ListarObraxCargo(int idUsuario, string cargo = "")
        {
            InitializeComponent();
            IdUsuario = idUsuario;
            Cargo = cargo;

            ObraxCargoItems = new ObservableCollection<ObraxCargoItem>();
            CargarObraResponsableItems();

            // Comandos
            RefreshObraxCargoCommand = new Command(() =>
            {
                ObraxCargoItems.Clear();
                CargarObraResponsableItems();
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

        private async void CargarObraResponsableItems()
        {
            try
            {
                WebService servicio = new WebService();
                object[,] variables = new object[,] { { "idResponsable", IdUsuario } };
                dynamic obras = await servicio.MetodoPost("ServicioObra.asmx", "MostrarObrasResponsable", variables);

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
                                idObra = obra.idObra
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


        #region ================================ Scroll Infinito ================================
        /*
            @ Evento que se dispara cadaves que el escroll lega al final de ventana
            ================================
                    SCROLL INFINITO
            ================================
        */
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = listViewObraxCargo.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                // Aqui Logica de programacion cada ves que se ejecute este evento =====================================================//
                // int cargarNuevos = 5; // solo de prueva
                // int totalRegistroActual = PropietarioItems.Count(); // solo de prueva

            }
        }
        #endregion


    }
}
