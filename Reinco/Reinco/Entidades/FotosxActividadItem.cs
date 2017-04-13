using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Entidades
{
    public class FotosxActividadItem
    {
        public int id { get; set; }
        public string foto { get; set; }
        public Stream file { get; set; }
    }
}
