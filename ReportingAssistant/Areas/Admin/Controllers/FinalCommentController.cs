using Microsoft.AspNet.Identity;
using ReportingAssistant.Controllers;
using ReportingAssistant.Identity;
using ReportingAssistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ReportingAssistant.Areas.Admin.Controllers
{
    public class FinalCommentController : Controller
    {
        // GET: Admin/FinalComment
        CommonDataController cdc = new CommonDataController();
        ApplicationDbContext db = new ApplicationDbContext();
        //[MyActionFilter]
        //[MyResultFilter]
        //[MyExceptionFilter]



        public ActionResult FinalCDetailsEdit(long ids)
        {
            var appdbcontext = new ReportingDbContext();
            dropdown();

            var data = appdbcontext.FinalComments.Where(m => m.FinalCommentID == ids).ToList();

            FinalComment Data = new FinalComment();
            var userid = data[0].UserID;
            var adminid = data[0].AdminUserID;
            //var usename = db.Users.Where(m => m.Id == userid).Select(m => m.UserName).FirstOrDefault();
            //var Adminname = db.Users.Where(m => m.Id == adminid).Select(m => m.UserName).FirstOrDefault();
            Data.FinalCommentID = data[0].FinalCommentID;
            Data.Screen = data[0].Screen;
            Data.Description = data[0].Description;
            Data.ProjectID = data[0].ProjectID;
            Data.UserID = data[0].UserID;
            Data.AdminUserID = data[0].AdminUserID;
            Data.DateOfFinalComment = Convert.ToDateTime(data[0].DateOfFinalComment);
            Data.Attachment = data[0].Attachment;
            return View(Data);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalCDetailsEdit(FinalComment data, HttpPostedFileBase ImageAttached)
        {
            if (ModelState.IsValid)
            {

                var db = new ReportingDbContext();

                dropdown();
                string path = uploadImage(ImageAttached);
                if (path.Equals("-1"))
                {
                    Response.Write("<script>alert('Some error occured')</script>");
                }
                else
                {
                    FinalComment databaseData = db.FinalComments.Where(m => m.FinalCommentID == data.FinalCommentID).FirstOrDefault();
                    databaseData.AdminUserID = data.AdminUserID;
                    databaseData.Description = data.Description;
                    databaseData.ProjectID = data.ProjectID;
                    databaseData.Screen = data.Screen;
                    databaseData.FinalCommentID = data.FinalCommentID;
                    databaseData.UserID = data.UserID;
                    databaseData.DateOfFinalComment = Convert.ToDateTime(data.DateOfFinalComment);
                    databaseData.Attachment = path;
                    db.SaveChanges();
                    TempData["Message"] = cdc.Success("Details Updated Successfully");
                }

                return RedirectToAction("Index", "Home", new { Areas = "Administrator" });
            }
            else
            {
                ModelState.AddModelError("My error", "Invalid data");
                return View();
            }
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
        public void dropdown()
        {
            List<SelectListItem> UserList = new List<SelectListItem>();
            List<SelectListItem> AdminList = new List<SelectListItem>();
            var userstore = new ApplicationUserStore(db);
            ApplicationUserManager appman = new ApplicationUserManager(userstore);
            var user_data = db.Users.OrderBy(a => a.UserName).Select(m => new { m.Id, m.UserName }).ToList();

            if (user_data.Count > 0)
            {

                foreach (var Value in user_data)
                {
                    if (appman.IsInRole(Value.Id, "Admin"))
                    {
                        UserList.Add(new SelectListItem { Text = Value.UserName, Value = Value.Id.ToString() });
                    }
                    else
                    {
                        UserList.Add(new SelectListItem { Text = Value.UserName, Value = Value.Id.ToString() });
                    }

                }
                ViewBag.user_data = UserList;
                ViewBag.Admin_data = UserList;
            }
        }
    }
}