using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Models
{
    public class QuestionTag
    {
        [Key]
        public int QuestionId { get; set; }
        [Key]
        public int TagId { get; set; }
    }
}