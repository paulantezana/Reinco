using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string dni { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string correo { get; set; }
        public string cip { get; set; }
    }
}
