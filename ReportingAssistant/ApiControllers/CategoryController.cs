using ReportingAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReportingAssistant.ApiControllers
{
    public class CategoryController : ApiController
    {
        [HttpPost]
        public object DeleteCategory(long categoryID)
        {
            ReportingDbContext db = new ReportingDbContext();
            try
            {
                Category category = db.Categories.Where(c => c.CategoryID == categoryID).FirstOrDefault();
                db.Categories.Remove(category);
                db.SaveChanges();
                return Json(new { success = true, message = "Successfully deleteed the category." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
