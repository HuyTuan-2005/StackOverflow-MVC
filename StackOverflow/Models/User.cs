using System;
using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Models
{
    public class User
    {
        [Key]
        public int UserId{get; set;}
        public string UserName{get; set;}
        public string Password{get; set;}
        public string Email{get; set;}
        public DateTime CreatedAt{get; set;}
        public DateTime? LastActivity { get; set; }
    }
}