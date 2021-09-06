using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ChatModule
{
    public class LocationAPIController
    {
        public async Task<IpstackApiResult> CallApiAsync()
        {
            IpstackApiResult result = null;
            string URL = "http://api.ipstack.com/check";
            string urlParameters = "?access_key=745f0ee9cb257e0329e00017545b6ea0";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(urlParameters); 
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                result = await response.Content.ReadAsAsync<IpstackApiResult>();  //Make sure to add a reference to System.Net.Http.Formatting.dll
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();
            return result; 
        }
    }
}
