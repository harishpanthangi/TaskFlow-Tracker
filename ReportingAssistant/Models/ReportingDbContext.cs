using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReportingAssistant.Models
{
    public class ReportingDbContext:DbContext
    {
        public ReportingDbContext() : base("MyConnectionString")
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TasksDone> TasksDone { get; set; }
        public DbSet<FinalComment> FinalComments { get; set; }
    }
}