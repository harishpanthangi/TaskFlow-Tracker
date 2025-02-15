using ReportingAssistant.Controllers;
using ReportingAssistant.Identity;
using ReportingAssistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ReportingAssistant.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProjectController : Controller
    {
        // GET: Admin/Project
        CommonDataController cdc = new CommonDataController();
        ReportingDbContext db = new ReportingDbContext();
        ApplicationDbContext userDb = new ApplicationDbContext();
        
        public ActionResult Create()
        {
            dropdown();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Project data, HttpPostedFileBase BaseImg)
        {
            dropdown();
            ModelState.Remove("ProjectID");
            if (ModelState.IsValid)
            {
                string path = "-1";
                if (BaseImg != null && BaseImg.ContentLength>0)
                {
                    path = cdc.uploadImage(BaseImg);
                    if (path.Equals("-1"))
                    {
                        Response.Write("<script>alert('Some error occured')</script>");
                        return View();
                    }
                }
                var prdata = db.Projects.Create();
                    prdata.ProjectName = data.ProjectName;
                    prdata.DateOfStart = data.DateOfStart.Value.Date;
                    prdata.AvailabilityStatus = data.AvailabilityStatus;
                    prdata.CategoryID = data.CategoryID;
                    prdata.Photo = path.Equals("-1") ? null : path;
                    db.Projects.Add(prdata);
                    db.SaveChanges();
                    TempData["Message"] = cdc.Success("Details Created Successfully");
                    return RedirectToAction("ViewAll");
                
            }
            else
            {
                TempData["Message"] = cdc.Success("Invalid Data!");
                return View();
            }


        }
        public ActionResult Edit(long ids)
        {
            dropdown();
            var data = db.Projects.Where(m => m.ProjectID == ids).FirstOrDefault();
            if (data != null)
            {
                Project obj = new Project();
                obj.ProjectID = data.ProjectID;
                obj.ProjectName = data.ProjectName;
                obj.DateOfStart = data.DateOfStart;
                ViewBag.date = data.DateOfStart.Value.Date.ToString("dd-MM-yyyy");
                obj.AvailabilityStatus = data.AvailabilityStatus;
                obj.CategoryID = data.CategoryID;
                obj.Photo = data.Photo;
                return View(obj);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Project data, HttpPostedFileBase BaseImg)
        {
            dropdown();
            if (ModelState.IsValid)
            {
                string path = uploadImage(BaseImg);
                if (path != null)
                {
                    var details = db.Projects.Where(m => m.ProjectID == data.ProjectID).FirstOrDefault();
                    details.ProjectName = data.ProjectName;
                    details.CategoryID = data.CategoryID;
                    details.DateOfStart = data.DateOfStart.Value.Date;

                    details.AvailabilityStatus = data.AvailabilityStatus;
                    details.Photo = path;
                    db.SaveChanges();
                    TempData["Message"] = cdc.Success("Details Updated Successfully");
                    return RedirectToAction("ViewAll");
                }
                else
                {
                    TempData["Message"] = cdc.Warning("Please Select a Image");
                    return View();
                }
            }
            else
            {
                TempData["Message"] = cdc.Success("Invalid Data");
                return View();
            }

        }

        public ActionResult ViewAll(string search)
        {
            if (search == null)
            {
                List<Project> list = new List<Project>();
                var prodata = db.Projects.Include(p => p.Category).ToList();
                if (prodata != null)
                {
                    foreach (var item in prodata)
                    {
                        Project obj = new Project();
                        obj.ProjectID = item.ProjectID;
                        obj.ProjectName = item.ProjectName;
                        obj.DateOfStart = item.DateOfStart;
                        obj.Photo = item.Photo;
                        obj.CategoryID = item.CategoryID;
                        obj.AvailabilityStatus = item.AvailabilityStatus;
                        obj.Category = item.Category;
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
            else if (search != null)
            {

                var List = db.Projects.Where(e => e.ProjectName.Contains(search)).ToList();
                try
                {
                    if (List.Count > 0)
                    {
                        //var data = new { List, status = "success", message = "" };
                        //return Json(data, JsonRequestBehavior.AllowGet);
                        return View(List);
                    }
                    else
                    {

                        ModelState.AddModelError("My error", "No Records Found");
                        return View();
                    }

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("My error", "Due to some problem unable to fetch data");
                    return View();
                }

            }
            else
            {
                ModelState.AddModelError("My error", "Kindly refresh the page to date data");
                return View();
            }

        }


        [ChildActionOnly]
        public void dropdown()
        {
            List<SelectListItem> CategoryList = new List<SelectListItem>();
            var ct_data = db.Categories.OrderBy(a => a.CategoryName).Select(m => new { m.CategoryID, m.CategoryName }).ToList();
            if (ct_data.Count > 0)
            {

                foreach (var Value in ct_data)
                {
                    CategoryList.Add(new SelectListItem { Text = Value.CategoryName.ToString(), Value = Value.CategoryID.ToString() });


                }
                ViewBag.list = CategoryList;
            }
        }
        public string uploadImage(HttpPostedFileBase BaseImg)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (BaseImg != null && BaseImg.ContentLength > 0)
            {
                string extension = Path.GetExtension(BaseImg.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png") || extension.ToLower().Equals(".gif"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/UploadImage"), random + Path.GetFileName(BaseImg.FileName));
                        BaseImg.SaveAs(path);
                        path = "~/Content/UploadImage/" + random + Path.GetFileName(BaseImg.FileName);
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg, jpeg, pg and jif files are allowed')</script>");
                }
            }
            else
            {
                path = "-1";
                TempData["Message"] = cdc.Success("Please Select a Image");
            }

            return path;
        }

        [HttpPost]
        public ActionResult searchdata(string search)
        {
            {

                try
                {
                    if (search != "")
                    {
                        var List = db.Projects.Where(e => e.ProjectName.Contains(search)).ToList();
                        try
                        {
                            if (List.Count > 0)
                            {
                                //var data = new { List, status = "success", message = "" };
                                //return Json(data, JsonRequestBehavior.AllowGet);
                                return RedirectToAction("ViewAll", List);
                            }
                            else
                            {

                                var data = new { search, status = "fail", message = "No Record Found" };
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (Exception ex)
                        {
                            var data = new { status = "exception", message = "Due to some problem unable to fetch data" };
                            return RedirectToAction("ViewAll", List);
                        }
                    }
                    else
                    {
                        var status = "fail";
                        var message = "All fields are required !";
                        var data = new { status = status, message = message };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    var data = new { status = "Exception", message = ex.Message };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }


            }
        }
        [HttpPost]
        public ActionResult DeleteProject(long projectId)
        {
            try
            {
                Project project = db.Projects.Where(temp => temp.ProjectID == projectId).FirstOrDefault();
                db.Projects.Remove(project);
                db.SaveChanges();
                return Json(new { success = true, message = "Successfully deleteed the Project." });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}