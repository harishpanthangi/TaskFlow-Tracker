using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReportingAssistant.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username Is Required")]
        [Display(Name ="User Name")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [Display(Name ="Password")]
        public string password { get; set; }
    }
}