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
            this.urlBase = "http://" + App.ip + ":" + App.puerto + "/" + App.cuenta; // ejemplo: http://192.168.1.37:8080/reinco
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
        // USADO PARA: Llamar servicios web, que nos devuelva si tabla de datos
        public async Task<dynamic> MetodoGet(string servicio, string metodo, object[,] variables)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = string.Format("{0}/{1}/{2}", this.urlBase, servicio, metodo);
                for (int i = 0; i < variables.Length / 2; i++)
                {
                    if (i == 0)
                        url += "?" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                    else
                        url += "&" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                }
                string contenido;
                dynamic datosTabla;
                var cliente = new HttpClient();
                
              //  await Task.Run(() => cliente);//linea agregada para evitar bloqueo mutuo
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
                return  datosTabla;// await agregado
            }
            catch (Exception ex)
            {
                string aux = ex.ToString();
                throw;
            }
        }
      
        // USADO PARA: Llamar servicios web y que esperamos un mensaje de respuesta
        public async Task<string> MetodoGetString(string servicio, string metodo, object[,] variables)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = string.Format("{0}/{1}/{2}", this.urlBase, servicio, metodo);
                for (int i = 0; i < variables.Length / 2; i++)
                {
                    if (i == 0)
                        url += "?" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                    else
                        url += "&" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                }
                string contenido;
                var cliente = new HttpClient();
                var message = await cliente.GetAsync(url);
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    var json = await message.Content.ReadAsStringAsync();
                    contenido = Convert.ToString(json);
                }
                else
                {
                    contenido = message.ReasonPhrase.ToString();
                }
                return contenido;
            }
            catch (Exception ex)
            {
                string aux = ex.ToString();
                throw;
            }
        }
        //========================RECIBIR ARRAY DE IDENTIFICADORES ==================================
        public async Task<string> MetodoGetStringArray(string servicio, string metodo, object[,] variables)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = string.Format("{0}/{1}/{2}", this.urlBase, servicio, metodo);
                //for(var arreglo in variables[0,1])
                for (int i = 0; i < variables.Length / 2; i++)
                {
                    if (i == 0)
                        url += "?" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                    else
                        url += "&" + variables[i, 0].ToString() + "=" + variables[i, 1].ToString();
                }
                string contenido;
                var cliente = new HttpClient();
                var message = await cliente.GetAsync(url);
                if (message.StatusCode == HttpStatusCode.OK)
                {
                    var json = await message.Content.ReadAsStringAsync();
                    contenido = Convert.ToString(json);
                }
                else
                {
                    contenido = message.ReasonPhrase.ToString();
                }
                return contenido;
            }
            catch (Exception ex)
            {
                string aux = ex.ToString();
                throw;
            }
        }
        public async Task<dynamic> MetodoPost(string servicio, string metodo, object[,] variables)
        {
            try
            {
                // Formando la URL unicode resource lacator
                HttpClient client = new HttpClient();
                string url = string.Format("{0}/{1}/{2}", this.urlBase, servicio, metodo);

                // Encodificando Para el metodo POST
                var body = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < variables.Length / 2; i++)
                    body.Add(new KeyValuePair<string, string>(variables[i, 0].ToString(), variables[i, 1].ToString()));
                var content = new FormUrlEncodedContent(body);

                string contenido;
                dynamic datosTabla;
                var cliente = new HttpClient();
                var message = cliente.PostAsync(url, content).Result;

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
            catch (Exception ex)
            {
                 return ex.Message;
                throw;
            }
        }
        public async Task<string> MetodoPostStringImagenes(string servicio, string metodo, object[,] variables)
        {
            try
            {
                // Formando la URL unicode resource lacator
                HttpClient client = new HttpClient();
                string url = string.Format("{0}/{1}/{2}", this.urlBase, servicio, metodo);

                // Encodificando Para el metodo POST
                var body = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < variables.Length / 2; i++)
                    body.Add(new KeyValuePair<string, string>(variables[i, 0].ToString(), variables[i, 1].ToString()));
                // var content = new  (body);
                // StringContent content = new StringContent("data=" + HttpUtility.UrlEncode(action.Body), Encoding.UTF8, "application/x-www-form-urlencoded");
                var content = JsonConvert.SerializeObject(body);
                var content_2 = new StringContent(content);
                string contenido;
                //dynamic datosTabla;
                var cliente = new HttpClient();
                var message = cliente.PostAsync(url, content_2).Result;

                if (message.StatusCode == HttpStatusCode.OK)
                {
                    var json = await message.Content.ReadAsStringAsync();
                    contenido = Convert.ToString(json);
                }
                else
                {
                    contenido = message.ReasonPhrase.ToString();// mensaje de error
                }
                return contenido;
            }
            catch (Exception)
            {
                // return ex.Message;
                throw;
            }
        }
        #region===========recibe un string de respuesta=========================
        public async Task<string> MetodoPostString(string servicio, string metodo, object[,] variables)
        {
            try
            {
                // Formando la URL unicode resource lacator
                HttpClient client = new HttpClient();
                string url = string.Format("{0}/{1}/{2}", this.urlBase, servicio, metodo);

                // Encodificando Para el metodo POST
                var body = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < variables.Length / 2; i++)
                    body.Add(new KeyValuePair<string, string>(variables[i, 0].ToString(), variables[i, 1].ToString()));
                var content = new FormUrlEncodedContent(body);

                string contenido;
                var cliente = new HttpClient();
                var message = cliente.PostAsync(url, content).Result;

                if (message.StatusCode == HttpStatusCode.OK)
                {
                    var json = await message.Content.ReadAsStringAsync();
                    contenido = Convert.ToString(json);
                }
                else
                {
                    contenido = message.ReasonPhrase.ToString();
                }
                return contenido;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
        #endregion
    }
}