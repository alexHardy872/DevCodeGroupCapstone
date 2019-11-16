using DevCodeGroupCapstone.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevCodeGroupCapstone.Service_Classes
{
    public static class DistanceMatrix
    {
        public static double metersToMiles = 1609.34;

        public static async Task<Lesson> GetTravelInfo(Lesson lesson)
        {
            HttpClient client = new HttpClient();
            ApplicationDbContext context = new ApplicationDbContext();

            string requestUrl = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
            string destinationsString = "&destinations=";
            string authenticationString = "&key=" + Private.ApiKey.googleMapsApiKey;

            var teacher = context.People.Include("Location").Where(p => p.PersonId == lesson.teacherId).SingleOrDefault();
            var teacherPreference = context.Preferences.Where(p => p.teacherId == teacher.PersonId).SingleOrDefault();
            var tempLessonLocation = context.Locations.Where(l => l.LocationId == lesson.LocationId).SingleOrDefault();

            double[] teacherLatLng = new double[2];
            teacherLatLng[0] = double.Parse(teacher.Location.lat);
            teacherLatLng[1] = double.Parse(teacher.Location.lng);

            double[] lessonLocationLatLng = new double[2];
            lessonLocationLatLng[0] = double.Parse(tempLessonLocation.lat);
            lessonLocationLatLng[1] = double.Parse(tempLessonLocation.lng);

            var response = await client.GetStringAsync(requestUrl + teacherLatLng[0] + "," + teacherLatLng[1] + destinationsString + lessonLocationLatLng[0] + "," + lessonLocationLatLng[1] + authenticationString);

            JObject distanceInfo = JObject.Parse(response);

            if (teacherPreference.distanceType == RadiusOptions.Miles)
            {
                double tempDuration = (double)distanceInfo["rows"][0]["elements"][0]["distance"]["value"];
                lesson.travelDuration = Convert.ToInt32(tempDuration / metersToMiles);
            }
            else //minutes
            {
                double tempMinutes = (int)distanceInfo["rows"][0]["elements"][0]["duration"]["value"];
                lesson.travelDuration = Convert.ToInt32(Math.Floor(tempMinutes / 60));
            }


            return lesson;
        }
    }
}