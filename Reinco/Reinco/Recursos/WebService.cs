using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Reinco.Recursos
{
    public class WebService
    {
        //public string urlBase { get; set; }


        //public WebService()
        //{
        //    this.urlBase = "http://192.168.1.37";
        //}

        //public async Task get(string direccion)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        var url = string.Format("{0}{1}", this.urlBase, direccion);
        //        var response = await client.GetAsync(url);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return null;
        //        }
        //        var result = await response.Content.ReadAsStringAsync();
        //        dynamic content = JsonConvert.DeserializeObject(result);
        //        return content;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}


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


        //public async Task<List<T>> Get<T>(string urlBase, string servicePrefix = "", string controller = "")
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}{1}", servicePrefix, controller);
        //        var response = await client.GetAsync(url);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return null;
        //        }
        //        var result = await response.Content.ReadAsStringAsync();
        //        var list = JsonConvert.DeserializeObject<List<T>>(result);
        //        return list;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
    }
}
