using System;
using System.Collections.Generic;
using StackOverflow.Models;

namespace StackOverflow.ViewModels
{
    public class HomePageViewModel
    {
        public int QustionId {get; set;}
        public int UserId {get; set;}
        public string DisplayName {get; set;}
        public string Title {get; set;}
        public string Body {get; set;}
        public List<string> Tags {get; set;}
        public DateTime CreatedAt {get; set;}
        public int AnswerCount {get; set;}
    }
}