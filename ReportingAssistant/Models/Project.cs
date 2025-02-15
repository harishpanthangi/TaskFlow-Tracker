using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReportingAssistant.Models
{
    public class Project
    {
        [Key]
        public long ProjectID { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Project Name can only contain alphabets, digits, and spaces.")]
        public string ProjectName { get; set; }

        public DateTime? DateOfStart { get; set; }

        [Required]
        public string AvailabilityStatus { get; set; } 

        [ForeignKey("Category")]
        public long CategoryID { get; set; }
                                        
        public string Photo { get; set; }

        public virtual Category Category { get; set; }
    }
}