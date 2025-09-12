using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Models
{
    public class User
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName {get; set;}
        public string Password {get; set;}
    }
}