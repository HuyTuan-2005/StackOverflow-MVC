using System;
using System.Collections.Generic;
using StackOverflow.Models;

namespace StackOverflow.ViewModels
{
    public class HomePageViewModel
    {
        // public static int PostCount;
        
        public string DisplayName {get; set;}
        public string Title {get; set;}
        public string Body {get; set;}
        public List<string> Tags {get; set;}
        public DateTime CreatedAt {get; set;}
        public int AnswerCount {get; set;}
    }
}