using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Recursos
{
    public interface ReducirImagen
    {
        Task<byte[]> ResizeImage(byte[] imageData, float width, float height);
    }
}
