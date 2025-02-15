using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportingAssistant.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string UserImg { get; set; }
    }
}