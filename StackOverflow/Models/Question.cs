using System;
using System.Collections.Generic;
using System.Linq;

namespace StackOverflow.Models
{
    public class Question
    {
        public string DisplayName {get; set;}
        public string title{get; set;}
        public string body{get; set;}
        public List<Tag> tags{get; set;}
        public DateTime gioDang{get; set;}

        public List<Tag> AddTags(string ntag)
        {
            string[] lst = ntag.Split(',').Select(t => t.Trim()).ToArray();
            tags = new List<Tag>();

            foreach (var item in lst)
            {
                var tag = new Tag(item);
                tags.Add(tag);
            }
            return tags;
        }
    }
}