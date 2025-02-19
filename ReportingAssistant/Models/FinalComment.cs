﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReportingAssistant.Models
{
    public class FinalComment
    {
        [Key]
        public long FinalCommentID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Screen { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 2)]
        public string Description { get; set; }

        
        public string AdminUserID { get; set; } // FK from AspNetUsers

        [Required]
        public string UserID { get; set; } // FK from AspNetUsers

        
        public DateTime DateOfFinalComment { get; set; }

        public string Attachment { get; set; } // File path for uploaded attachment

        [ForeignKey("Project")]
        public long ProjectID { get; set; }
        public virtual Project Project { get; set; }

    }
}