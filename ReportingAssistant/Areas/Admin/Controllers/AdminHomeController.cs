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
    public class AdminHomeController : Controller
    {
        ReportingDbContext db = new ReportingDbContext();
        ApplicationDbContext userDb = new ApplicationDbContext();
        CommonDataController cdc = new CommonDataController();
        // GET: Admin/AdminHome
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Issues()
        {
            //String UserId = Convert.ToString(Session["UserId"]);


            List<Task> List = new List<Task>();
            List<TasksDone> tdoneList = new List<TasksDone>();
            List<FinalComment> fcmntlist = new List<FinalComment>();
            TasksDone alldata = new TasksDone();
            Task Lstdata = new Task();
            DateTime fromdate = Convert.ToDateTime(System.DateTime.Now.AddDays(-7).ToString("dd/MMM/yyyy"));
            DateTime todate = Convert.ToDateTime(System.DateTime.Now.ToString("dd/MMM/yyyy") + " 23:59");
            HttpCookie AdminID = Request.Cookies["UserId"];
            dropdown();
            if (AdminID == null)
            {
                return RedirectToAction("Logout", "AdminHome");
            }
            else
            {
                var Adminid = AdminID.Value;

                var User_Details = userDb.Users.ToList();
                foreach (var i in User_Details)
                {

                    var TAdata = db.Tasks.Where(m => m.DateOfTask >= fromdate && m.DateOfTask <= todate && m.UserID == i.Id).ToList();
                    if (TAdata != null)
                    {
                        foreach (var item in TAdata)
                        {


                            var obj = new Task();
                            obj.TaskID = item.TaskID;
                            obj.Screen = item.Screen;
                            obj.Description = item.Description;
                            obj.UserID = i.Id; //here UserName is there but I modified it to Id
                            obj.DateOfTask = item.DateOfTask;
                            obj.AdminUserID = item.AdminUserID;
                            List.Add(obj);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("My error", "No Records Found");
                        return View();
                    }

                    var tdone = db.TasksDone.Where(m => m.UserID == i.Id && m.DateOfTaskDone >= fromdate && m.DateOfTaskDone <= todate).ToList();
                    if (tdone != null)
                    {
                        foreach (var item2 in tdone)
                        {

                            var tddata = new TasksDone();
                            tddata.TaskDoneID = item2.TaskDoneID;
                            tddata.Screen = item2.Screen;
                            tddata.Project = item2.Project;
                            tddata.ProjectID = item2.ProjectID;
                            tddata.Attachment = item2.Attachment;
                            tddata.DateOfTaskDone = item2.DateOfTaskDone;
                            tddata.UserID = i.Id;//UserNAme
                            tddata.Description = item2.Description;
                            tddata.AdminID = item2.AdminID;
                            tdoneList.Add(tddata);

                        }
                    }
                    else
                    {

                        ModelState.AddModelError("My error", "No Records Found");
                        return View();
                    }

                    var fcomments = db.FinalComments.Where(m => m.UserID == i.Id && m.DateOfFinalComment >= fromdate && m.DateOfFinalComment <= todate).ToList();
                    if (fcomments != null)
                    {
                        foreach (var item3 in fcomments)
                        {

                            var fcdata = new FinalComment();
                            fcdata.FinalCommentID = item3.FinalCommentID;
                            fcdata.Screen = item3.Screen;
                            fcdata.Project = item3.Project;
                            fcdata.ProjectID = item3.ProjectID;
                            fcdata.Attachment = item3.Attachment;
                            fcdata.UserID = i.Id;
                            fcdata.AdminUserID = item3.AdminUserID;
                            fcdata.DateOfFinalComment = item3.DateOfFinalComment;
                            fcdata.Description = item3.Description;
                            fcmntlist.Add(fcdata);

                        }

                    }
                    else
                    {

                        ModelState.AddModelError("My error", "No Records Found");
                        return View();
                    }

                    //string AdminuserId = data[0].AdminUserId.ToString();
                    //foreach (var item in TaskList)
                    //{
                    //    var UserName = userDb.Users.Where(m => m.Id == item.AdminUserId.ToString()).ToList();
                    //    AdminNamelist.Add(UserName);
                    //    ViewBag.AdminNameList = AdminNamelist;
                    //}


                }
                alldata.AdminID = Adminid.ToString();
                alldata.tlist = List.ToList();
                alldata.tdlist = tdoneList.ToList();
                alldata.fclist = fcmntlist.ToList();


                return View(alldata);
            }

        }
        public void dropdown()
        {
            List<SelectListItem> ProjectList = new List<SelectListItem>();
            var pr_data = db.Projects.OrderBy(a => a.ProjectName).Select(m => new { m.ProjectID, m.ProjectName }).ToList();
            if (pr_data.Count > 0)
            {

                foreach (var Value in pr_data)
                {
                    ProjectList.Add(new SelectListItem { Text = Value.ProjectName, Value = Value.ProjectID.ToString() });


                }
                ViewBag.pr_data = ProjectList;
            }
            List<SelectListItem> UserList = new List<SelectListItem>();
            var user_data = userDb.Users.OrderBy(a => a.UserName).Select(m => new { m.Id, m.UserName }).ToList();
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
        public ActionResult Insertd_data(FinalComment fcdata, HttpPostedFileBase BaseImg)
        {
            if (ModelState.IsValid)
            {
                string Adminid = Convert.ToString(Session["UserId"]);
                try
                {
                    if (fcdata.ProjectID.ToString() != "" && fcdata.UserID != "")
                    {
                        string path = uploadImage(BaseImg);
                        var data = db.FinalComments.Create();
                        try
                        {
                            data.ProjectID = fcdata.ProjectID;
                            data.Screen = fcdata.Screen;
                            data.UserID = fcdata.UserID;
                            data.AdminUserID = Adminid;
                            data.Description = fcdata.Description;
                            data.DateOfFinalComment = DateTime.Now;
                            data.Attachment = path;
                            db.FinalComments.Add(data);
                            db.SaveChanges();
                            TempData["Message"] = cdc.Success("The data saved successfully!");
                            return RedirectToAction("Issues");
                            //var data2 = new { status = "Success", message = "Data Added Successfully!!" };
                            //return Json(data2, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            //var data2 = new { status = "fail", message = "Please try after some time!!" };
                            //return Json(data2, JsonRequestBehavior.AllowGet);
                            TempData["Message"] = cdc.Success("Some Issue Appeared!");
                            return RedirectToAction("Issues");
                        }
                    }
                    else
                    {
                        TempData["Message"] = cdc.Success("Some Issue Appeared!");
                        return RedirectToAction("Issues");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = cdc.Success("Some Issue Appeared!");
                    return RedirectToAction("Issues");
                }
            }
            else
            {
                TempData["Message"] = cdc.Success("Some Issue Appeared!");
                return RedirectToAction("Issues");
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
        public ActionResult Logout()
        {
            var authmanager = HttpContext.GetOwinContext().Authentication;
            authmanager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}