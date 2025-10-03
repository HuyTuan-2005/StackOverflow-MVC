using System;
using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Models
{
    public class Answer
    {
        [Key]
        public int AnswerId{get; set;}
        
        public int QuestionId{get; set;}
        public int UserId{get; set;}
        public string Body{get; set;}
        public DateTime CreatedAt{get; set;}
    }
}