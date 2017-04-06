using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Entidades
{
  public  interface IPhoto
    {
        Task<Stream> GetPhoto();
    }
}
