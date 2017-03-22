using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Services
{
    public class ApiService
    {
        public async Task<List<T>> Get<T>(string urlBase)
        {
            try
            {
                var client = new HttpClient();
                // client.BaseAddress = new Uri(urlBase);
                // var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(urlBase);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string result = "[{'idUsuario':3,'dni':'23334334','nombres':'administrador','apellidos':'admin','usuario':'admin','contrasena':'admin','correo':'correo','cip':'014'}]";

                // var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                // var list = JsonConvert.DeserializeObject<List<T>>(result);
                return list;
            }
            catch
            {
                return null;
            }
        }

    }
}
