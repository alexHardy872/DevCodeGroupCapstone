using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DevCodeGroupCapstone.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        //public string timestamp { get; set; }
        //public string text { get; set; }
        ////tlc

        //[ForeignKey("Person1")]//fk attr
        //public int senderId { get; set; } //fk's spot at the table
        //public Person Person1 { get; set; }//the class the fk attr is referencing
        //[ForeignKey("Person2")]//fk attr
        //public int recipientId { get; set; }
        //public Person Person2 { get; set; }//the class the fk attr is referencing
    }
}