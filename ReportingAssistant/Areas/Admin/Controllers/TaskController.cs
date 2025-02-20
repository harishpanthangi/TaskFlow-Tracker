using ReportingAssistant.Controllers;
using ReportingAssistant.Identity;
using ReportingAssistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportingAssistant.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class TaskController : Controller
    {
        // GET: Admin/Task
        CommonDataController cdc = new CommonDataController();
        ApplicationDbContext db = new ApplicationDbContext();
        ReportingDbContext rdb = new ReportingDbContext();
        //[MyActionFilter]
        //[MyResultFilter]
        //[MyExceptionFilter]

        public ActionResult CreateTaskDetails()
        {
            dropdown();
            return View();
        }
        [HttpPost]
        public ActionResult CreateTaskDetails(Task data, HttpPostedFileBase ImageAttached)
        {
            ApplicationDbContext userdb = new ApplicationDbContext();
            if (ModelState.IsValid)
            {

                var db = new ReportingDbContext();
                dropdown();
                HttpCookie AdminID = Request.Cookies["UserId"];

                if (AdminID == null)
                {
                    return RedirectToAction("Logout", "AdminHome");
                }
                else
                {
                    var Admin = AdminID.Value;
                    var AdminName = userdb.Users.Where(m => m.Id == Admin).Select(m => m.UserName).FirstOrDefault();
                    var UserName = userdb.Users.Where(m => m.Id == data.UserID).Select(m => m.UserName).FirstOrDefault();

                    
                    string path = uploadImage(ImageAttached);
                    if (path.Equals("-1"))
                    {
                        Response.Write("<script>alert('Some error occured')</script>");
                    }
                    else
                    {
                        Task databaseData = db.Tasks.Create();
                        databaseData.AdminUserID = Admin;
                        databaseData.Description = data.Description;
                        databaseData.ProjectID = data.ProjectID;
                        databaseData.Screen = data.Screen;
                        databaseData.TaskID = data.TaskID; 
                        databaseData.UserID = data.UserID;
                        databaseData.DateOfTask = Convert.ToDateTime(data.DateOfTask);
                        databaseData.Attachment = path;
                        databaseData.UserName = UserName;
                        databaseData.AdminName = AdminName;
                        db.Tasks.Add(databaseData);
                        db.SaveChanges();
                        TempData["Message"] = cdc.Success("Details Created Successfully");
                    }
                }
                return RedirectToAction("ViewTaskDetails", "Task", new { area = "Admin" });
            }
            else
            {
                ModelState.AddModelError("My error", "Invalid data");
                return View();
            }
        }





        public ActionResult TaskDetailsEdit(long ids)
        {
            var appdbcontext = new ReportingDbContext();

            dropdown();
            var data = appdbcontext.Tasks.Where(m => m.TaskID == ids).ToList();
            var userid = data[0].UserID;
            var adminid = data[0].AdminUserID;
            var usename = db.Users.Where(m => m.Id == userid).Select(m => m.UserName).FirstOrDefault();
            var Adminname = db.Users.Where(m => m.Id == adminid).Select(m => m.UserName).FirstOrDefault();
            Task Data = new Task();
            Data.TaskID = data[0].TaskID;
            Data.Screen = data[0].Screen;
            Data.Description = data[0].Description;
            Data.ProjectID = data[0].ProjectID;
            Data.UserID = userid;//here username is there
            Data.AdminUserID = adminid;//here adminname is there
            Data.DateOfTask = Convert.ToDateTime(data[0].DateOfTask);
            Data.Attachment = data[0].Attachment;
            return View(Data);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaskDetailsEdit(Task data, HttpPostedFileBase ImageAttached)
        {
            if (ModelState.IsValid)
            {

                var db = new ReportingDbContext();
                HttpCookie AdminID = Request.Cookies["UserId"];
                dropdown();
                string path = uploadImage(ImageAttached);
                if (path.Equals("-1"))
                {
                    Response.Write("<script>alert('Some error occured')</script>");
                }
                else
                {
                    Task databaseData = db.Tasks.Where(m => m.TaskID == data.TaskID).FirstOrDefault();
                    databaseData.AdminUserID = AdminID.Value;
                    databaseData.Description = data.Description;
                    databaseData.ProjectID = data.ProjectID;
                    databaseData.Screen = data.Screen;
                    databaseData.TaskID = data.TaskID;
                    databaseData.UserID = data.UserID;
                    databaseData.DateOfTask = Convert.ToDateTime(data.DateOfTask);
                    databaseData.Attachment = path;
                    db.SaveChanges();
                    TempData["Message"] = cdc.Success("Details Updated Successfully");
                }

                return RedirectToAction("ViewTaskDetails", "Task", new { area = "Admin" });
            }
            else
            {
                ModelState.AddModelError("My error", "Invalid data");
                return View();
            }
        }

        public ActionResult ViewTaskDetails()
        {

            List<Task> listdata = new List<Task>();
            ReportingDbContext db = new ReportingDbContext();
            ApplicationDbContext userdb = new ApplicationDbContext();
            var data = db.Tasks.ToList();
            foreach (var item in data)
            {
                var AdminName = userdb.Users.Where(m => m.Id == item.AdminUserID).ToList();
                var UserName = userdb.Users.Where(m => m.Id == item.UserID).ToList();
                var Taskdata = new Task();
                Taskdata.TaskID = item.TaskID;
                Taskdata.Screen = item.Screen;
                Taskdata.Description = item.Description;
                Taskdata.ProjectID = item.ProjectID;
                Taskdata.UserName = UserName[0].UserName;
                Taskdata.DateOfTask = Convert.ToDateTime(item.DateOfTask);
                Taskdata.Attachment = item.Attachment;
                Taskdata.AdminName = AdminName[0].UserName;

                listdata.Add(Taskdata);
            }
            return View(listdata);
        }

        [NonAction]
        public string uploadImage(HttpPostedFileBase ImageAttached)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (ImageAttached != null && ImageAttached.ContentLength > 0)
            {
                string extension = Path.GetExtension(ImageAttached.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png") || extension.ToLower().Equals(".gif"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/UploadImage"), random + Path.GetFileName(ImageAttached.FileName));
                        ImageAttached.SaveAs(path);
                        path = "~/Content/UploadImage/" + random + Path.GetFileName(ImageAttached.FileName);
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
                Response.Write("<script>alert('Please Select a file')</script>");
            }

            return path;
        }

        //public string saveImage(string BaseImg)
        //{
        //    string filename = Guid.NewGuid().ToString() + ".jpg";
        //    System.Drawing.Image ImgFile = cdc.Base64ToImage(BaseImg);
        //    var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedFile/"), filename);
        //    ImgFile.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    return filename;
        //}
        public void dropdown()
        {
            List<SelectListItem> ProjectList = new List<SelectListItem>();
            var pr_data = rdb.Projects.OrderBy(a => a.ProjectName).Select(m => new { m.ProjectID, m.ProjectName }).ToList();
            if (pr_data.Count > 0)
            {

                foreach (var Value in pr_data)
                {
                    ProjectList.Add(new SelectListItem { Text = Value.ProjectName, Value = Value.ProjectID.ToString() });


                }
                ViewBag.pr_data = ProjectList;
            }
            List<SelectListItem> UserList = new List<SelectListItem>();
            var user_data = db.Users.OrderBy(a => a.UserName).Select(m => new { m.Id, m.UserName }).ToList();
            if (user_data.Count > 0)
            {

                foreach (var Value in user_data)
                {
                    UserList.Add(new SelectListItem { Text = Value.UserName, Value = Value.Id.ToString() });


                }
                ViewBag.user_data = UserList;
            }
        }
        [HttpPost]
        public ActionResult DeleteTask(long taskId)
        {
            try
            {
                Task task = rdb.Tasks.Where(temp => temp.TaskID == taskId).FirstOrDefault();
                rdb.Tasks.Remove(task);
                db.SaveChanges();
                return Json(new { success = true, message = "Successfully deleteed the Task." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}