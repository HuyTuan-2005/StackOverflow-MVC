using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflow.Models
{
    public class IdentityModels : IdentityUser
    {
        // chỉ thêm 2 thuộc tính này vì những thuộc tính khác đã có sẵn trong IdentityUser
        public DateTime CreatedAt { get; set; }
        public DateTime LastActivity { get; set; }
    }
}