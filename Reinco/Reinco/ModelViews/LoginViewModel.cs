using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.ModelViews
{
    public class LoginViewModel
    {
        #region Contrctor
        public LoginViewModel()
        {
            // recordar = true;
        }
        #endregion

        public string nombreUsuario { get; set; }
        public string password { get; set; }
        // public bool recordar { get; set; }

    }
}
