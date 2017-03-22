using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Recursos
{
    public class DialogService
    {
        public async Task MostrarMensaje(string titulo, string mensaje)
        {
            await App.Current.MainPage.DisplayAlert(titulo, mensaje, "Aceptar");
        }
    }
}
