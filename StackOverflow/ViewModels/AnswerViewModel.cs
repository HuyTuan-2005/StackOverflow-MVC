using System;

namespace StackOverflow.ViewModels
{
    public class AnswerViewModel
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; } 
    }
}