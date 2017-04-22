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
        public int incidencia { get; set; }
        public int acumulado { get; set; }
    }
}
