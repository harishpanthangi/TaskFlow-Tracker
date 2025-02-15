using ReportingAssistant.Controllers;
using ReportingAssistant.Identity;
using ReportingAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ReportingAssistant.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        CommonDataController cdc = new CommonDataController();
        ReportingDbContext db = new ReportingDbContext();
        ApplicationDbContext userDb = new ApplicationDbContext();
        // GET: /Admin/Category/
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category data)
        {
            if (ModelState.IsValid)
            {
                var savedata = db.Categories.Create();
                savedata.CategoryName = data.CategoryName;
                db.Categories.Add(savedata);
                db.SaveChanges();
                TempData["Message"] = cdc.Success("Details Created Successfully");
                return RedirectToAction("ViewAll");
            }
            else
            {
                TempData["Message"] = cdc.Success("Invalid Data!");
                return RedirectToAction("ViewAll");
            }


        }
        public ActionResult Edit(long ids)
        {
            var data = db.Categories.Where(m => m.CategoryID == ids).FirstOrDefault();
            if (data != null)
            {
                Category obj = new Category();
                obj.CategoryID = data.CategoryID;
                obj.CategoryName = data.CategoryName;
                return View(obj);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Category data)
        {
            if (ModelState.IsValid)
            {
                var details = db.Categories.Where(m => m.CategoryID == data.CategoryID).FirstOrDefault();
                details.CategoryName = data.CategoryName;
                db.SaveChanges();
                TempData["Message"] = cdc.Success("Details Updated Successfully");
                return RedirectToAction("ViewAll");
            }
            else
            {
                TempData["Message"] = cdc.Success("Invalid Data");
                return RedirectToAction("ViewAll");
            }

        }

        public ActionResult ViewAll()
        {
            List<Category> list = new List<Category>();
            var data = db.Categories.ToList();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Category obj = new Category();
                    obj.CategoryID = item.CategoryID;
                    obj.CategoryName = item.CategoryName;
                    list.Add(obj);

                }
                return View(list);
            }
            else
            {
                ModelState.AddModelError("My error", "No Records Found");
                return View();

            }

        }

        [ChildActionOnly]
        public string Msgbar(string errMsg, int correct)
        {
            if (correct > 0)
            {
                errMsg = correct.ToString() + "  record(s) Deleted Successfully!";
                TempData["Message"] = cdc.Success(errMsg);
            }
            //else if (correct == 0 && incorret > 0)
            //{
            //    errMsg = incorret.ToString() + "  record(s) failed to Delete due to existence of records!";
            //    TempData["Message"] = cdc.Danger(errMsg);
            //}
            //else if (correct > 0 && incorret > 0)
            //{
            //    errMsg = correct.ToString() + "record(s) Deleted Successfully and  " + incorret.ToString() + "  record(s) failed to Delete due to existence of records!";
            //    TempData["Message"] = cdc.Warning(errMsg);
            //}
            return errMsg;
        }
        
    }
}