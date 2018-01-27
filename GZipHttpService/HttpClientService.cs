using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GZipHttpService
{
    public class HttpClientService
    {
        public static async Task<HttpResponseMessage> InsertDataAsync<T>(T data, string url, string id = null)
        {

            HttpResponseMessage response;
            try
            {
                // Simply create an instance of the HttpGZipClientHandler class
                using (var gzip = new HttpGZipClientHandler())
                {
                    // Pass it as the handler for the HttpClient instance
                    using (var client = new HttpClient(gzip))
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));

                        string json = JsonConvert.SerializeObject(data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        if (id != null) url = $"{url}/{id}";

                        response = await client.PostAsync(url, content);
                    }
                }
            }
            catch (HttpRequestException)
            {
                response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
            catch (Exception)
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        public static async Task<HttpResponseMessage> UpdateDataAsync<T>(T data, string url, string id = null)
        {

            HttpResponseMessage response;
            try
            {
                using (var gzip = new HttpGZipClientHandler())
                {
                    using (var client = new HttpClient(gzip))
                    {
                        string json = JsonConvert.SerializeObject(data, Formatting.None);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        if (id != null) url = $"{url}/{id}";
                        response = await client.PutAsync(url, content);

                    }
                }
            }
            catch (HttpRequestException)
            {
                response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
            catch (Exception e)
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return response;
        }
    }
}
