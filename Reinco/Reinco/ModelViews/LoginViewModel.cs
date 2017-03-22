using GalaSoft.MvvmLight.Command;
using Reinco.Models;
using Reinco.Paginas;
using Reinco.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace Reinco.ModelViews
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Contrctor
        public LoginViewModel()
        {
            dialogService = new DialogService();
            apiService = new ApiService();
        }
        #endregion

        #region Atributos
        private DialogService dialogService;
        private ApiService apiService;
        private bool running;
        #endregion

        #region Propiedades
        public string nombreUsuario { get; set; }
        public string password { get; set; }
        // public bool recordar { get; set; }
        public bool isRunning {
            set
            {
                if(running != value)
                {
                    running = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("isRunning"));
                }
            }
            get
            {
                return running;
            }
        }
        public ICommand loginCommand { get { return new RelayCommand(Login); } } 
        #endregion

        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Comandos
        private async void Login()
        {
            #region Validacion De campos de campos vacios en el login
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                await dialogService.MostrarMensaje("Iniciar Sessión", "Debes Ingresar Un Nombre de usuario");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                await dialogService.MostrarMensaje("Iniciar Sessión", "Debes Ingresar La Contraseña");
                return;
            }
            #endregion
            isRunning = true;
            var usuario = await apiService.Get<Usuario>("http://192.168.1.37:8091/ServicioUsuario.asmx/MostrarUsuarioLogueo?usuario=admin&contrase%C3%B1a=admin");
            
            isRunning = false;

            App.Current.MainPage = new PaginaUsuario();
        } 
        #endregion
    }
}
