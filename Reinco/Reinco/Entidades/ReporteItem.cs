using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Entidades
{
    public class ReporteItem
    {
        public string direccionPath { get; set; }

        public string descripcion { get; set; }
        public int si { get; set; }
        public int no { get; set; }
        public int lev { get; set; }
        public string incidencia { get; set; }
        public string acumulado { get; set; }
       // public string acumularSumas { get; set; }
    }
}
