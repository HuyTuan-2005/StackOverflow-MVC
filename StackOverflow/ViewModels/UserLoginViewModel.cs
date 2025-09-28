using System.ComponentModel.DataAnnotations;
using StackOverflow.Models;

namespace StackOverflow.ViewModels
{
    public class UserLoginViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "* Enter your Username")]
        public string UserName {get; set;}
        
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "* Enter your Password")]
        public string Password {get; set;}
    }
}