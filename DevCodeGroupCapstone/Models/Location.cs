using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevCodeGroupCapstone.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }

        [Display(Name = "Address Line 1")]
        public string address1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string address2 { get; set; }

        [Display(Name = "City")]
        public string city { get; set; }

        [Display(Name = "State")]
        public StateList state { get; set; }

        [DataType(DataType.PostalCode)]
        [Display(Name = "ZIP code")]
        public string zip { get; set; }
        
        public string description { get; set; }
    }
}