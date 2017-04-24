using Reinco.Entidades;
using Reinco.Recursos;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarObra : ContentPage, INotifyPropertyChanged
    {
        string Color;
        int ultimoId = 100000;
        int NroElementos = App.nroElementos;
        int IdUsuario;
        bool mostrarTodo = false;
        bool mostrarResponsable = false;
        bool mostrarResponsableActivas = false;
        bool mostrarAsistente = false;
        bool mostrarAsistenteActivas = false;
        #region +---- Services -------
        HttpClient Cliente = new HttpClient();
        WebService Servicio = new WebService();
        #endregion

        #region +---- Eventos ----+
        new public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region +---- Atributos ----+
        public VentanaMensaje mensaje;
        string Mensaje;
        private bool isRefreshingObra { get; set; }
        #endregion

        #region +---- Propiedades ----+
        public ObservableCollection<ObraItem> ObraItems { get; set; }
        public bool IsRefreshingObra
        {
            set
            {
                    if (isRefreshingObra != value)
                    {
                        isRefreshingObra = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingObra"));
                    }
                }
                get
            {
                    return isRefreshingObra;
                }
           
        }
        #endregion

        #region +---- Comandos ----+
        public ICommand MostrarTodo { get; private set; }
        public ICommand MostrarActivas { get; private set; }
        public ICommand editarObra { get; private set; }
        public ICommand CrearObra { get; private set; }
        public ICommand RefreshObraCommand { get; private set; }
        #endregion

        #region +---- Constructor ----+
        public ListarObra()//-------------------Gerente
        {
            InitializeComponent();
            directorio.Text = App.directorio + "/Obras";

            ObraItems = new ObservableCollection<ObraItem>();
            //Carga las obras activas solamente
            CargarObraItems();
            #region +---- Preparando Los Comandos ----+
            // Muestra todas las obras
            MostrarTodo = new Command(() =>
            {
                mostrarTodo = true;//activamos el mostrar todas las obras
                ultimoId = 10000;
                ObraItems.Clear();
                CargarTodasObras(NroElementos,ultimoId);
            });
            //Solo muestra las obras activas
            MostrarActivas = new Command(() =>
            {
                mostrarTodo = false;
                ObraItems.Clear();
                CargarObraItems();
            });
            //Crear obras
            CrearObra = new Command(() =>
            {
                Navigation.PushAsync(new AgregarObra());
            });
            // Evento Refrescar La Lista
            try {
                RefreshObraCommand = new Command(() =>
                {
                    try
                    {
                        ultimoId = 10000;
                        ObraItems.Clear();
                        if (mostrarTodo == true)
                        {
                            CargarTodasObras(NroElementos, ultimoId);
                        }
                        else
                            CargarObraItems();
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("", ex.Message, "ok");
                    }
                });
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message, "ok");
            }
            #endregion

            this.BindingContext = this; // Contexto de los Bindings Clase Actual Importante para que pueda funcionar el refresco de la lista con Gestos
        }
        
        public ListarObra(int idUsuario)//---------------Usuario Responsable
        {
            IdUsuario = idUsuario;
            InitializeComponent();

            ObraItems = new ObservableCollection<ObraItem>();
            CargarObraItemsActivas(idUsuario,NroElementos,ultimoId);
            mostrarResponsableActivas = true;

            #region +---- Preparando Los Comandos ----+
            // Evento Crear Obra
            MostrarTodo = new Command(() =>
            {
                mostrarResponsable = true;
                mostrarResponsableActivas = false;
                ultimoId = 10000;
                ObraItems.Clear();
                CargarObraItems(idUsuario,NroElementos,ultimoId);
            });
            MostrarActivas = new Command(() =>
            {
                mostrarResponsable = false;
                mostrarResponsableActivas = true;
                ultimoId = 10000;
                ObraItems.Clear();
                CargarObraItemsActivas(idUsuario, NroElementos, ultimoId);
            });
            // Evento Refrescar La Lista
            RefreshObraCommand = new Command(() =>
            {
                int ultimoId = 100000;
                ObraItems.Clear();
                if(mostrarResponsableActivas==true)
                     CargarObraItemsActivas(idUsuario,NroElementos,ultimoId);
                if(mostrarResponsable==true)
                    CargarObraItems(idUsuario, NroElementos, ultimoId);
                IsRefreshingObra = false;
            });
            #endregion

            this.BindingContext = this; // Contexto de los Bindings Clase Actual Importante para que pueda funcionar el refresco de la lista con Gestos
        }
        public ListarObra(int idUsuario,string cargo)//----------Usuario asistente 
        {
            InitializeComponent();
            ObraItems = new ObservableCollection<ObraItem>();
            IdUsuario = idUsuario;
            
            CargarObraItemsAsistenteActivas(idUsuario,NroElementos,ultimoId);
            mostrarAsistenteActivas = true;
            MostrarTodo = new Command(() =>
            {
                mostrarAsistente = true;
                mostrarAsistenteActivas = false;
                ultimoId = 100000;
                ObraItems.Clear();
                CargarObraItemsAsistente(idUsuario,NroElementos, ultimoId);
            });
            MostrarActivas = new Command(() =>
            {
                mostrarAsistente = false;
                mostrarAsistenteActivas = true;
                ultimoId = 100000;
                ObraItems.Clear();
                CargarObraItemsAsistenteActivas(idUsuario, NroElementos, ultimoId);
            });
            #region +---- Preparando Los Comandos ----+
            RefreshObraCommand = new Command(() =>
            {
                ultimoId = 100000;
                ObraItems.Clear();
                if(mostrarAsistenteActivas==true)
                    CargarObraItemsAsistenteActivas(idUsuario, NroElementos, ultimoId);
                if(mostrarAsistente==true)
                    CargarObraItemsAsistente(idUsuario, NroElementos, ultimoId);
                IsRefreshingObra = false;
            });
            #endregion

            this.BindingContext = this; // Contexto de los Bindings Clase Actual Importante para que pueda funcionar el refresco de la lista con Gestos
        }
        #endregion

        #region +---- Definiendo Propiedad Global De esta Pagina ----+
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                App.ListarObra = this;
            }
            catch(Exception ex) {
                DisplayAlert("", ex.Message, "Ok");
            }
        }
        #endregion

        #region +---- Cargando las obras ----+
        #region==============Solo obras activas===========================================
        public async void CargarObraItems()
        {
            try
            {
               IsRefreshingObra = true;
                //servicioObra, mostrarObras--modificado
                dynamic obras = await Servicio.MetodoGet("ServicioPropietarioObra.asmx", "MostrarObrasActivas");
                foreach (var item in obras)
                {
                    
                        if (item.idPropietario == null || item.idUsuario_responsable == null)
                        {
                            Color = "#FF7777";
                        }
                        else
                            Color = "#77FF77";

                        ObraItems.Add(new ObraItem
                        {
                            idObra = item.idObra,
                            nombre = item.nombre,
                            codigo = item.codigo,
                            idPropietario = item.idPropietario == null ? 0 : item.idPropietario,
                            idUsuario = item.idUsuario_responsable == null ? 0 : item.idUsuario_responsable,
                            colorObra = Color,
                            idPropietarioObra = item.idPropietario_Obra == null ? 0 : item.idPropietario_Obra,
                            nombrePropietario = item.nombrePropietario,
                            nombresApellidos = item.nombresApellidos,
                            finalizada = item.supervision_terminada == null ? 0 : item.supervision_terminada,
                            ocultar = true,

                        });
                    }
                   
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
                return;
            }
            finally
            {
                IsRefreshingObra = false;
            }
        }
        #endregion
        #region=================todas las obras=========================================
        public async void CargarTodasObras(int elementos, int ultimo)
        {
            try
            {
                IsRefreshingObra = true;
                //servicioObra, mostrarObras--modificado
                object[,] OidPlantilla = new object[,] { { "nroElementos", elementos }, { "ultimoId", ultimo } };
                dynamic obras = await Servicio.MetodoGet("ServicioPropietarioObra.asmx", "MostrarPropietarioObraDetalle", OidPlantilla);
                foreach (var item in obras)
                {
                   
                    if (item.idPropietario == null || item.idUsuario_responsable == null)
                    {
                        Color = "#c94036";//ambar, obra sin responsable o propietario
                    }
                    else
                        Color = "#e09833";
                    if (item.supervision_terminada == 1)
                        Color = "#77FF77";
                    ObraItems.Add(new ObraItem
                        {
                            idObra = item.idObra,
                            nombre = item.nombre,
                            codigo = item.codigo,
                            idPropietario = item.idPropietario == null ? 0 : item.idPropietario,
                            idUsuario = item.idUsuario_responsable == null ? 0 : item.idUsuario_responsable,
                            colorObra = Color,
                            idPropietarioObra = item.idPropietario_Obra == null ? 0 : item.idPropietario_Obra,
                            nombrePropietario = item.nombrePropietario,
                            nombresApellidos = item.nombresApellidos,
                            finalizada = item.supervision_terminada == null ? 0 : item.supervision_terminada,
                            ocultar = true,

                        });
                    }
                
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                IsRefreshingObra = false;
            }
            ultimoId = ObraItems[ObraItems.Count - 1].idObra;
        }

        #endregion
        #region=============obras responsable=======================================
        public async void CargarObraItems(int idUsuario, int elementos, int ultimo)
        {
            try
            {
                //servicioObra, mostrarObras--modificado
                object[,] OidPlantilla = new object[,] { { "idResponsable", idUsuario }, { "nroElementos", elementos }, { "ultimoId", ultimo } };
                dynamic obras = await Servicio.MetodoGet("ServicioPropietarioObra.asmx", "MostrarResponsablexObra", OidPlantilla);
                foreach (var item in obras)
                {
                    
                    if (item.idPropietario == null || item.idUsuario_responsable == null)
                    {
                        Color = "#FF7777";
                    }
                    else
                        Color = "#77FF77";
                    ObraItems.Add(new ObraItem
                    {
                        idObra = item.idObra,
                        nombre = item.nombre,
                        codigo = item.codigo,
                        idPropietario = item.idPropietario == null ? 0 : item.idPropietario,
                        idUsuario = item.idUsuario_responsable == null ? 0 : item.idUsuario_responsable,
                        colorObra = Color,
                        idPropietarioObra = item.idPropietario_Obra,
                        nombrePropietario = item.nombrePropietario==null?"": item.nombrePropietario,
                        nombresApellidos = item.responsable==null?"": item.responsable,
                        ocultar=true
                    });
                }
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
                return;
            }
            ultimoId = ObraItems[ObraItems.Count - 1].idObra;
        }
        public async void CargarObraItemsActivas(int idUsuario, int elementos, int ultimo)
        {
            try
            {
                //servicioObra, mostrarObras--modificado
                object[,] OidPlantilla = new object[,] { { "idResponsable", idUsuario }, { "nroElementos", elementos }, { "ultimoId", ultimo } };
                dynamic obras = await Servicio.MetodoGet("ServicioPropietarioObra.asmx", "MostrarResponsablexObraActivas", OidPlantilla);
                foreach (var item in obras)
                {
                    if (item.idPropietario == null || item.idUsuario_responsable == null)
                    {
                        Color = "#FF7777";
                    }
                    else
                        Color = "#77FF77";
                    ObraItems.Add(new ObraItem
                    {
                        idObra = item.idObra,
                        nombre = item.nombre,
                        codigo = item.codigo,
                        idPropietario = item.idPropietario == null ? 0 : item.idPropietario,
                        idUsuario = item.idUsuario_responsable == null ? 0 : item.idUsuario_responsable,
                        colorObra = Color,
                        idPropietarioObra = item.idPropietario_Obra,
                        nombrePropietario = item.nombrePropietario == null ? "" : item.nombrePropietario,
                        nombresApellidos = item.responsable == null ? "" : item.responsable,
                        ocultar = true
                    });
                }
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
                return;
            }
            ultimoId = ObraItems[ObraItems.Count - 1].idObra;
        }
        #endregion
        #region===================obras asistente=============================
        public async void CargarObraItemsAsistente(int idUsuario, int elementos, int ultimo)
        {
            try
            {
                
                //servicioObra, mostrarObras--modificado
                object[,] OidPlantilla = new object[,] { { "idUsuario", idUsuario }, { "nroElementos", elementos }, { "ultimoId", ultimo } };
                dynamic obras = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarObrasSupervision", OidPlantilla);
                foreach (var item in obras)
                {
                    if (item.idPropietario == null || item.idUsuario_responsable == null)
                    {
                        Color = "#FF7777";
                    }
                    else
                        Color = "#77FF77";
                    App.correo = item.correo;//correo del responsable
                    ObraItems.Add(new ObraItem
                    {
                        idObra = item.idObra,
                        nombre = item.nombre,
                        codigo = item.codigo,
                        ocultar = false,
                        colorObra = Color,
                    });
                }
               
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
                return;
            }
            ultimoId = ObraItems[ObraItems.Count - 1].idObra;
        }
        int contador = 0;
        public async void CargarObraItemsAsistenteActivas(int idUsuario, int elementos, int ultimo)
        {
            try
            {
                
                //servicioObra, mostrarObras--modificado
                object[,] OidPlantilla = new object[,] { { "idUsuario", idUsuario }, { "nroElementos", elementos }, { "ultimoId", ultimo } };
                dynamic obras = await Servicio.MetodoGet("ServicioUsuario.asmx", "MostrarObrasSupervisionActivas", OidPlantilla);
                if (obras != null)
                {
                    if (obras.Count == 0&& contador==0) //si está vacío
                    {
                        //await DisplayAlert("Obras", "No hay obras por supervisar", "Aceptar");
                        return;
                    }
                    else
                    {
                        foreach (var item in obras)
                        {
                            if (item.idPropietario == null || item.idUsuario_responsable == null)
                            {
                                Color = "#FF7777";
                            }
                            else
                                Color = "#77FF77";
                            App.correo = item.correo;//correo del responsable
                            ObraItems.Add(new ObraItem
                            {
                                idObra = item.idObra,
                                nombre = item.nombre,
                                codigo = item.codigo,
                                ocultar = false,
                                colorObra = Color,
                            });
                        }
                        contador++;
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Obras", "Error de respuesta del servicio, Contáctese con el administrador.", "Aceptar");
                    return;
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
                return;
            }
            ultimoId = ObraItems[ObraItems.Count - 1].idObra;
        }
        #endregion
        #endregion

        #region +---- Evento Eliminar Obra ----+
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {

                var idObra = ((MenuItem)sender).CommandParameter;
                int IdObra = Convert.ToInt16(idObra);
                bool respuesta = await DisplayAlert("Eliminar", "Eliminar idObra = " + idObra, "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idObra", IdObra } };
                dynamic result = await Servicio.MetodoGetString("ServicioObra.asmx", "EliminarObra", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Obra", Mensaje, "OK");

                    // Recargando La lista
                    ObraItems.Clear();
                    CargarObraItems();
                    // 
                    return;
                }
                //
                // Evento Refrescar La Lista
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Obra", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region ================================ Scroll Infinito ================================
        
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                var items = listViewObras.ItemsSource as IList;
                if (items != null && e.Item == items[items.Count - 1])
                {
                    if (mostrarTodo == true)
                        //===mostramos todas las obras del gerente
                        CargarTodasObras(NroElementos, ultimoId);
                    if (mostrarResponsable == true)
                        //=========Mostramos todas las obras del responsable
                        CargarObraItems(IdUsuario, NroElementos, ultimoId);
                    if (mostrarResponsableActivas == true)
                        //========Mostramos solo las obras activas del responsable
                        CargarObraItemsActivas(IdUsuario, NroElementos, ultimoId);
                    if (mostrarAsistente == true)
                    {
                        //=========Mostramos todas las obras del asistente
                        CargarObraItemsAsistente(IdUsuario, NroElementos, ultimoId);
                    }
                    if (mostrarAsistenteActivas == true)
                    {
                        //========Mostramos solo las obras activas del asistente
                        CargarObraItemsAsistenteActivas(IdUsuario, NroElementos, ultimoId);
                    }

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message, "Ok");
            }
            
         //   mostrarAsistente = false;
        }
        #endregion
        protected override bool OnBackButtonPressed()
        {
            App.Navigator.Detail = new NavigationPage(new PaginaUsuario());
            return true;
            
        }
    }
}
