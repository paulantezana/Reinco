using Reinco.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Reinco
{
    public partial class App : Application
    {
        public App()
        {
            //paul comentario nuevo GitHub
            /*========================================
             ==================================
             ========================*/
            //Jorge: Comnentario agregado para ver la sincronización
            InitializeComponent();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
