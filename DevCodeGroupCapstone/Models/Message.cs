using System.ComponentModel.DataAnnotations;

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
        //public int senderId { get; set; } 
        //public Person Person1 { get; set; }
        //[ForeignKey("Person2")]//fk attr
        //public int recipientId { get; set; }
        //public Person Person2 { get; set; }
    }
}