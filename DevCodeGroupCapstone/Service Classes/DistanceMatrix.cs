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
            var teacherPreference = context.Preferences.Where(p => p.teacherId == lesson.teacherId).SingleOrDefault();
            var tempLessonLocation = context.Locations.Where(l => l.LocationId == lesson.LocationId).SingleOrDefault();

            if (tempLessonLocation == null)
            {
                Person student = context.People
                    .Include("Location")
                    .Where(s => s.PersonId == lesson.studentId).FirstOrDefault();

                tempLessonLocation = student.Location;
            }

            double[] teacherLatLng = new double[2];
            teacherLatLng[0] = double.Parse(teacher.Location.lat);
            teacherLatLng[1] = double.Parse(teacher.Location.lng);

            double[] lessonLocationLatLng = new double[2];
            lessonLocationLatLng[0] = double.Parse(tempLessonLocation.lat);
            lessonLocationLatLng[1] = double.Parse(tempLessonLocation.lng);

            var response = await client.GetStringAsync(requestUrl + teacherLatLng[0] + "," + teacherLatLng[1] + destinationsString + lessonLocationLatLng[0] + "," + lessonLocationLatLng[1] + authenticationString);


            JObject distanceInfo = JObject.Parse(response);
            // this MIGHT have to go 
            //if (teacherPreference.distanceType != RadiusOptions.Miles)
            //{
                //double tempduration = (double)distanceInfo["rows"][0]["elements"][0]["distance"]["value"];
                //lesson.travelDuration = Convert.ToInt32(tempduration / metersToMiles);
                double tempDistance = (double)distanceInfo["rows"][0]["elements"][0]["distance"]["value"];
                lesson.travelDistance = Convert.ToInt32(tempDistance / metersToMiles);
            //}
            //else //minutes
            //{
                double tempMinutes = (int)distanceInfo["rows"][0]["elements"][0]["duration"]["value"]; // throws error?

            //lesson.travelDuration = Convert.ToInt32(Math.Floor(tempMinutes / 60));
            lesson.travelDuration = Convert.ToInt32(Math.Floor((tempMinutes / 60)));
            //}


            return lesson;
        }

        public static async Task<int> GetTravelInfo(Person student, Person teacher)
        {
            Lesson lesson = new Lesson();
            lesson.studentId = student.PersonId;
            lesson.teacherId = teacher.PersonId;

            lesson = await GetTravelInfo(lesson);

            if (lesson.travelDuration > 0)
            {
                return lesson.travelDuration;
            }
            else
            {
                return 0;
            }
        }
    }
}