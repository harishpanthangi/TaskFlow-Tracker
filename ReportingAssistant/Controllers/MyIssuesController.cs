using ReportingAssistant.Identity;
using ReportingAssistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportingAssistant.Controllers
{
    [Authorize]
    public class MyIssuesController : Controller
    {
        ReportingDbContext db = new ReportingDbContext();
        ApplicationDbContext userDb = new ApplicationDbContext();
        CommonDataController cdc = new CommonDataController();
        // GET: /Admin/MyIssues/
        public ActionResult Issues()
        {
            dropdown();
            String UserId = Convert.ToString(Session["UserId"]);
            if (UserId == null)
            {
                return RedirectToAction("Account", "Logout");
            }
            else
            {
                List<Task> List = new List<Task>();
                List<TasksDone> tdoneList = new List<TasksDone>();
                List<FinalComment> fcmntlist = new List<FinalComment>();
                TasksDone alldata = new TasksDone();
                Task Lstdata = new Task();
                DateTime fromdate = Convert.ToDateTime(System.DateTime.Now.AddDays(-1).ToString("dd/MMM/yyyy"));
                DateTime todate = Convert.ToDateTime(System.DateTime.Now.ToString("dd/MMM/yyyy") + " 23:59");
                if (UserId != null)
                {
                    //var data = (from t in db.Task
                    //join user in userDb.Users on t.AdminUserId equals user.Id where(t.UserId==UserId && user.Id==UserId)
                    //select new { t.Description, t.Screen, user.UserName }).ToList();
                    //var data = db.Task.Join(userDb.Users, p => p.AdminUserId == UserId, pc => pc.Id == UserId, (p, pc) => new { p.Screen, p.Description, pc.UserName }).ToList(); ;

                    //var data = db.Tasks.Where(m => m.UserID == UserId && m.DateOfTask >= fromdate && m.DateOfTask <= todate).ToList();
                    var data = db.Tasks
                        .Select(t => new {
                            t.TaskID,
                            t.Screen,
                            t.Description,
                            t.AdminUserID,
                            t.UserID,
                            t.DateOfTask,
                            t.Attachment,
                            t.ProjectID,
                            t.UserName
                        })
                        .Where(m => m.UserID == UserId)
                        .ToList();
                    foreach (var item in data)
                    {
                        var admin_name = userDb.Users.Where(m => m.Id == item.AdminUserID).Select(m => m.UserName).FirstOrDefault();//removed date condition
                        var obj = new Task();
                        obj.Screen = item.Screen;
                        obj.Description = item.Description;
                        obj.UserID = item.UserID;
                        obj.AdminUserID = item.AdminUserID;
                        obj.DateOfTask = item.DateOfTask;
                        obj.AdminName = admin_name;
                        obj.UserName = item.UserName;
                        List.Add(obj);
                    }
                    var tdone = db.TasksDone.Where(m => m.UserID == UserId).ToList();// && m.DateOfTaskDone >= fromdate && m.DateOfTaskDone <= todate comntd in where cndtn
                    foreach (var item in tdone)
                    {
                        var tddata = new TasksDone();
                        tddata.Screen = item.Screen;
                        tddata.Project = item.Project;
                        tddata.ProjectID = item.ProjectID;
                        tddata.Attachment = item.Attachment;
                        tddata.DateOfTaskDone = item.DateOfTaskDone;
                        tddata.Description = item.Description;
                        tdoneList.Add(tddata);

                    }
                    var fcomments = db.FinalComments.Where(m => m.UserID == UserId).ToList();// && m.DateOfFinalComment >= fromdate && m.DateOfFinalComment <= todate in where cndtn
                    foreach (var item in fcomments)
                    {
                        var admin_name = userDb.Users.Where(m => m.Id == item.AdminUserID).Select(m => m.UserName).FirstOrDefault();
                        var fcdata = new FinalComment();
                        fcdata.Screen = item.Screen;
                        fcdata.Project = item.Project;
                        fcdata.ProjectID = item.ProjectID;
                        fcdata.Attachment = item.Attachment;
                        fcdata.AdminUserID = admin_name;
                        fcdata.DateOfFinalComment = item.DateOfFinalComment;
                        fcdata.Description = item.Description;
                        fcmntlist.Add(fcdata);

                    }
                    //string AdminuserId = data[0].AdminUserId.ToString();
                    //foreach (var item in TaskList)
                    //{
                    //    var UserName = userDb.Users.Where(m => m.Id == item.AdminUserId.ToString()).ToList();
                    //    AdminNamelist.Add(UserName);
                    //    ViewBag.AdminNameList = AdminNamelist;
                    //}

                    alldata.tlist = List.ToList();
                    alldata.tdlist = tdoneList.ToList();
                    alldata.fclist = fcmntlist.ToList();
                }
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
        }

        [HttpPost]
        public ActionResult Insertd_data(TasksDone tddata, HttpPostedFileBase BaseImg)
        {
            if (ModelState.IsValid)
            {

                string userid = Convert.ToString(Session["UserId"]);

                try
                {
                    if (tddata.ProjectID.ToString() != "" && userid != "")
                    {
                        string path = uploadImage(BaseImg);
                        var data = db.TasksDone.Create();
                        try
                        {
                            data.ProjectID = tddata.ProjectID;
                            data.Screen = tddata.Screen;
                            data.UserID = userid;
                            data.Description = tddata.Description;
                            data.DateOfTaskDone = DateTime.Now;
                            data.Attachment = path;
                            db.TasksDone.Add(data);
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
    }
}