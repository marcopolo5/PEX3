using Domain.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignalRClientModule
{
    public class LocationAPIController
    {
        /// <summary>
        /// Async method. Calls the location API and returns the user's location based on his IP.
        /// </summary>
        /// <returns>The location as an IpstackApiResult</returns>
        public async Task<IpstackApiResult> CallApiAsync()
        {
            IpstackApiResult result = null;
            var URL = "http://api.ipstack.com/check";
            var urlParameters = "?access_key=745f0ee9cb257e0329e00017545b6ea0";

            var client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(urlParameters); 
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
