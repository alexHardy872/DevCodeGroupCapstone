using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevCodeGroupCapstone.Models.View_Models
{
    public class PersonAndLocationViewModel
    {
        public Person person;

        public Location location;

        public class GeocoderViewModel
        {
            public Customer customer { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string address { get; set; }


           

        


        
    }
}