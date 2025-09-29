using System.ComponentModel.DataAnnotations;
using StackOverflow.Models;

namespace StackOverflow.ViewModels
{
    public class UserLoginViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string UserName {get; set;}
        
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password {get; set;}
    }
}