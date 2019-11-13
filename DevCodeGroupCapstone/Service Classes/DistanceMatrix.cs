using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevCodeGroupCapstone.Models;
using DevCodeGroupCapstone.Private;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevCodeGroupCapstone.Service_Classes
{
    public static class DistanceMatrix
    {
        public static async Task<int> GetTravelDuration(string teacherLat, string teacherLng, string lessonLat, string lessonLng)
        {
            HttpClient client = new HttpClient();
            string requestUrl = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
            string destinationsString = "&destinations=";
            string authenticationString = "&key=" + Private.ApiKey.googleMapsApiKey;

            double[] teacherLatLng = new double[2];
            teacherLatLng[0] = double.Parse(teacherLat);
            teacherLatLng[1] = double.Parse(teacherLng);

            double[] studentLatLng = new double[2];
            studentLatLng[0] = double.Parse(lessonLat);
            studentLatLng[1] = double.Parse(lessonLng);
            
            var response = await client.GetStringAsync(requestUrl + teacherLatLng[0] + "," + teacherLatLng[1] + destinationsString + studentLatLng[0] + "," + studentLatLng[1] + authenticationString);

            JObject distanceInfo = JObject.Parse(response);

            int lessonTravelDuration = (int)distanceInfo["rows"][0]["elements"][0]["duration"]["value"];

            return lessonTravelDuration;
        }
    }
}