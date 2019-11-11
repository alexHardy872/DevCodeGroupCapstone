using DevCodeGroupCapstone.Models;
using DevCodeGroupCapstone.Private;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;


namespace DevCodeGroupCapstone.Service_Classes
{
    public class GeoCode
    {

        public async Task<string[]> GetLongLatFromApi(string address)
        {          
            string[] latLng = new string[2];

            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", Uri.EscapeDataString(address), ApiKey.secretKey);

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            dynamic stuff = await Task.Run(() => JObject.Parse(content));
            var results = stuff["results"];
            var geometry = results[0]["geometry"];
            var location = geometry["location"];
            string lat = Convert.ToString(location["lat"]);
            string lng = Convert.ToString(location["lng"]);

            latLng[0] = lat;
            latLng[1] = lng;


            return latLng;
        }


        public string FormatAddress(Location location)
        {
            string address = location.address1 + " " + location.address2 + " " + location.city + " " + location.state + " " + location.zip;
            return address;
        }


    }
}