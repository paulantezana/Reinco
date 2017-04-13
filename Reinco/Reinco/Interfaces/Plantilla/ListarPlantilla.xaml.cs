﻿using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarPlantilla : ContentPage, INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;
        WebService Servicio = new WebService();

        public ObservableCollection<PlantillaItem> PlantillaItems { get; set; }
        private bool isRefreshingPlantilla { get; set; }
        public bool IsRefreshingPlantilla
        {
            set
            {
                if (isRefreshingPlantilla != value)
                {
                    isRefreshingPlantilla = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingPlantilla"));
                }
            }
            get
            {
                return isRefreshingPlantilla;
            }
        }
        public ICommand RefreshPlantillaCommand { get; private set; }
        public ICommand agregarPlantilla { get; private set; }
        public ListarPlantilla()
        {
            InitializeComponent();
            directorio.Text = App.directorio + "\\Plantilla";

            PlantillaItems = new ObservableCollection<PlantillaItem>();
            CargarPlantilla();

            // Commands
            RefreshPlantillaCommand = new Command(() =>
            {
                PlantillaItems.Clear();
                CargarPlantilla();
            });
            agregarPlantilla = new Command(() =>
            {
                Navigation.PushAsync(new AgregarPlantilla());
            });

            // Contexto Para Los bindings
            this.BindingContext = this;
        }

        #region +---- Propiedad Global De esta Pagina ----+
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ListarPlantilla = this;
        }
        #endregion

        public async void CargarPlantilla()
        {
            try
            {
                IsRefreshingPlantilla = true;
                dynamic plantillas = await Servicio.MetodoGet("ServicioPlantilla.asmx", "MostrarPlantillas");
                foreach (var plantilla in plantillas)
                {
                    PlantillaItems.Add(new PlantillaItem
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
            finally
            {
                IsRefreshingPlantilla = false;
            }
        }
    }
}
