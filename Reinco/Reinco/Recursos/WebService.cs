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
                var cliente = new HttpClient();
                var message = await cliente.GetAsync(url);
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
                for (int i = 0; i < variables.Length/2; i++)
                {
                    if (i == 0)
                        url += "?" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                    else
                        url += "&" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                }
                string contenido;
                dynamic datosTabla;
                var cliente = new HttpClient();
                var message = await cliente.GetAsync(url);
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    contenido = await message.Content.ReadAsStringAsync();
                    //intento deserealizar si es un json si no se puede debe ser un texto sin formato json y eso lo muestro
                    try
                    {
                        //[{"idUsuario": 3}], [], mensaje de error de la base de datos u otro error
                        datosTabla = JsonConvert.DeserializeObject(contenido);
                    }
                    catch
                    {
                        datosTabla = null;
                    }
                }
                else
                {
                    datosTabla = null;// mensaje de error
                }
                return datosTabla;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<dynamic> MetodoPost(string servicio, string metodo, object[,] variables)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = string.Format("{0}/{1}/{2}", this.urlBase, servicio, metodo);
                var body = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < variables.Length/2; i++)
                    body.Add(new KeyValuePair<string, string>(variables[i, 0].ToString(), variables[i, 1].ToString()));
                var content = new FormUrlEncodedContent(body);

                string contenido;
                dynamic datosTabla;
                var cliente = new HttpClient();
                var message = await cliente.PostAsync(url, content);
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    contenido = await message.Content.ReadAsStringAsync();
                    //intento deserealizar si es un json si no se puede debe ser un texto sin formato json y eso lo muestro
                    try
                    {
                        //[{"idUsuario": 3}], [], mensaje de error de la base de datos u otro error
                        datosTabla = JsonConvert.DeserializeObject(contenido);
                    }
                    catch
                    {
                        datosTabla = null;
                    }
                }
                else
                {
                    datosTabla = null;// mensaje de error
                }
                return datosTabla;
            }
            catch (Exception)
            {
                throw;
            }
        }

}
}
