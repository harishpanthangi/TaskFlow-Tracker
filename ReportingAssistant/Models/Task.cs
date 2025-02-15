using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReportingAssistant.Models
{
    public class Task
    {
        [Key]
        public long TaskID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Screen { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 2)]
        public string Description { get; set; }

        
        public string AdminUserID { get; set; } // FK from AspNetUsers

        [Required]
        public string UserID { get; set; } // FK from AspNetUsers

        [Required]
        public DateTime DateOfTask { get; set; }

        public string Attachment { get; set; } // File path for uploaded attachment

        [ForeignKey("Project")]
        public long ProjectID { get; set; }
        public virtual Project Project { get; set; }
        public string UserName { get; set; }
        public string AdminName { get; set; }
        [NotMapped]
        public List<Task> TaskList { get; set; }
    }
}