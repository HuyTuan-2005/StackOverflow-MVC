using System;
using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId{get; set;}
        public int UserId{get; set;}
        public string DisplayName{get; set;}
        public DateTime Birthday{get; set;}
        public string Gender{get; set;}
    }
}