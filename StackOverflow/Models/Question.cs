using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StackOverflow.Models
{
    public class Question
    {
        [Key]
        public int QustionId{get; set;}
        public int UserId{get; set;}
        public string Title{get; set;}
        public string Body{get; set;}
        public DateTime CreatedAt{get; set;}
    }
}