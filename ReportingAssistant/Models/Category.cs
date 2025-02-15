using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReportingAssistant.Models
{
    public class Category
    {
        [Key]
        public long CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Category Name can only contain alphabets, digits, and spaces.")]
        public string CategoryName { get; set; }

    }
}