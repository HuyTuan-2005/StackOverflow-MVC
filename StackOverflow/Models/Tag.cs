
using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Models
{
    public class Tag
    {
        [Key]
        public int TagId{get; set;}
        public string TagName{get; set;}
    }
}