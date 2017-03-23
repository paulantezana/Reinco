using GalaSoft.MvvmLight.Command;
using Reinco.Gestores;
using Reinco.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace Reinco.ModelViews
{
    public class LoginViewModel 
    {
        //#region Contrctor
        //public LoginViewModel()
        //{
        //    dialogService = new DialogService();
        //    apiService = new ApiService();
        //}
        //#endregion

        //#region Atributos
        //private DialogService dialogService;
        //private ApiService apiService;
        //private bool running;
        //#endregion

        //#region Propiedades
        //public string nombreUsuario { get; set; }
        //public string password { get; set; }
        //// public bool recordar { get; set; }
        //public bool isRunning {
        //    set
        //    {
        //        if(running != value)
        //        {
        //            running = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("isRunning"));
        //        }
        //    }
        //    get
        //    {
        //        return running;
        //    }
        //}
        //public ICommand loginCommand { get { return new RelayCommand(Login); } } 
        //#endregion

        //#region Eventos
        //public event PropertyChangedEventHandler PropertyChanged;
        //#endregion

        //#region Comandos
        //private async void Login()
        //{
        //    #region Validacion De campos de campos vacios en el login
        //    if (string.IsNullOrEmpty(nombreUsuario))
        //    {
        //        await dialogService.MostrarMensaje("Iniciar Sessión", "Debes Ingresar Un Nombre de usuario");
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(password))
        //    {
        //        await dialogService.MostrarMensaje("Iniciar Sessión", "Debes Ingresar La Contraseña");
        //        return;
        //    }
        //    #endregion
        //    isRunning = true; // efecto loading true
        //    string peticion = "?usuario=" + nombreUsuario + "&contrasenia=" + password;
        //    // var usuario = await apiService.Get<Usuario>("http://192.168.1.37:8081/ServicioUsuario.asmx/LogueoUsuarioAdmin", peticion);
        //    isRunning = false; // efecto loading false

        //    // Validacion de suario y contraseña
        //    //if (usuario == null)
        //    //{
        //    //    await dialogService.MostrarMensaje("Iniciar Sesion", "Nombre de usuario o contraseña incorrecta");
        //    //    return;
        //    //}
        //    await dialogService.MostrarMensaje("Iniciar Session", "Ok Todo Correcto");
        //    App.Current.MainPage = new PaginaUsuario();
        //} 
        //#endregion
    }
}
