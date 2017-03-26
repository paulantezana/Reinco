using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Recursos
{
    public class WebService
    {
        #region Propiedades
        public string urlBase { get; set; }
        #endregion

        #region Constructor
        public WebService()
        {
            this.urlBase = "http://" + App.ip + ":" + App.puerto; // ejemplo: http://192.168.1.37:8080
        }
        #endregion
        //ejemplo: ServicioUsuario.asmx, Login
        public async Task<dynamic> MetodoGet(string servicio, string metodo)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = string.Format("{0}/{1}/{2}", this.urlBase, servicio, metodo);
                string contenido;// = "{\"mensaje:error\"}";
                dynamic datosTabla;// = JsonConvert.DeserializeObject(contenido);
                var httpClient = new HttpClient();
                HttpResponseMessage message = (HttpResponseMessage)await httpClient.GetAsync(url);
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    contenido = await message.Content.ReadAsStringAsync();
                    datosTabla = JsonConvert.DeserializeObject(contenido);
                }
                else
                {
                    datosTabla = message.StatusCode.ToString();// devolvería un string?
                }
                return datosTabla;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<dynamic> MetodoGet(string servicio, string metodo, object[,] variables)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = string.Format("{0}/{1}/{2}", this.urlBase, servicio, metodo);
                for (int i = 0; i < variables.Rank; i++)
                {
                    if (i == 0)
                        url += "?" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                    else
                        url += "&" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                }
                string contenido;
                dynamic datosTabla;
                var httpClient = new HttpClient();
                HttpResponseMessage message = (HttpResponseMessage)await httpClient.GetAsync(url);
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    contenido = await message.Content.ReadAsStringAsync();
                    //intento deserealizar si es un json si no se puede debe ser un texto sin formato json y eso lo muestro
                    try
                    {
                        datosTabla = JsonConvert.DeserializeObject(contenido);
                    }
                    catch
                    {
                        datosTabla = contenido;//devuelve un siemple mensaje
                    }
                }
                else
                {
                    datosTabla = message.StatusCode.ToString();// mensaje de error
                }
                return datosTabla;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task post(string direccion)
        //{
        //    try
        //    {
        //        var body = new List<KeyValuePair<string, string>>
        //        {
        //            new KeyValuePair<string, string>("usuario", "admin"),
        //            new KeyValuePair<string, string>("contrasenia", "admin")
        //        };
        //        var content = new FormUrlEncodedContent(body);

        //        var client = new HttpClient();
        //        var url = string.Format("{0}{1}", this.urlBase, direccion);
        //        var response = await client.PostAsync(url, content);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return null;
        //        }
        //        var result = await response.Content.ReadAsStringAsync();
        //        dynamic contenido = JsonConvert.DeserializeObject(result);
        //        return contenido;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

    }
}
