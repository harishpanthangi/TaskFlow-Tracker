using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReportingAssistant.Models
{
    public class changepassword
    {
        public string UserName { get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
        [Compare("newpass", ErrorMessage = "Password and confirm password should be same")]
        public string ConfirmPassword { get; set; }
    }
}