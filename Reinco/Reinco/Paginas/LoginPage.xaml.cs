using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            startSession.Clicked += StartSession_Clicked;
        }

        private void StartSession_Clicked(object sender, EventArgs e)
        {
            #region Validacion Login Temporal
            string userTemporal = "admin";
            string passwordTemporal = "admin";
            if (string.IsNullOrEmpty(userName.Text) && string.IsNullOrEmpty(password.Text))
            {
                errorLoginMessage.Text = "Debe completar la informacion";
                return;
            }
            if (userName.Text != userTemporal && password.Text != passwordTemporal)
            {
                errorLoginMessage.Text = "Nombre de usuario o contraseña incorrecta intentelo nuevamente";
                return;
            }
            App.Current.MainPage = new MainPage();
            #endregion
        }
    }
}
