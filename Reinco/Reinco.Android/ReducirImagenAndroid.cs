using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Reinco;
using Reinco.Droid;
using Reinco.Recursos;
using Xamarin.Forms;
using System.IO;
using Android.Graphics;
using System.Threading.Tasks;

[assembly: Dependency(typeof(ReducirImagenAndroid))]
namespace Reinco.Droid
{
    public class ReducirImagenAndroid:ReducirImagen
    {
        public async Task<byte[]> ResizeImage(byte[] imageData, float width, float height)
        {
            return ResizeImageAndroid(imageData, width, height);
        }
        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}