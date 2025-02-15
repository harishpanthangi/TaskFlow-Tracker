using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportingAssistant.Controllers
{
    public class CommonDataController : Controller
    {
        // GET: CommonData
        public const string script = "<script> setTimeout(function(){ $('.alert').fadeOut('slow');},5000); </script>";
        public string Success(string message)
        {
            return script + "<div class='alert alert-success alert-dismissable text-center'><i class='fa fa-check-square-o fa-fw'></i>" + message + "<button class='close' aria-hidden='true' data-dismiss='alert' type='button'>×</button></div>";
        }

        [ChildActionOnly]
        public string Information(string message)
        {
            return script + "<div class='alert alert-info alert-dismissable text-center'><i class='fa fa-info-circle fa-fw'></i><button class='close' aria-hidden='true' data-dismiss='alert' type='button'>×</button>" + message + "</div>";
        }

        [ChildActionOnly]
        public string Warning(string message)
        {
            return script + "<div class='alert alert-warning alert-dismissable text-center'><i class='fa fa-warning fa-fw'></i><button class='close' aria-hidden='true' data-dismiss='alert' type='button'>×</button>" + message + "</div>";
        }
        [ChildActionOnly]
        public string Danger(string message)
        {
            return script + "<div class='alert alert-danger alert-dismissable text-center'><i class='fa fa-times fa-fw'></i><button class='close' aria-hidden='true' data-dismiss='alert' type='button'>×</button>" + message + "</div>";
        }

        [ChildActionOnly]
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZ123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [ChildActionOnly]
        public System.Drawing.Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String.Trim().Replace("data:image/jpeg;base64,", "").Replace("data:image/png;base64,", "").Replace("data:image/gif;base64,", ""));
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        [ChildActionOnly]
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
                TempData["Message"] = Success("Please Select a Image");
            }

            return path;
        }


        [ChildActionOnly]
        public void ClearCookies()
        {


            string[] myCookies = System.Web.HttpContext.Current.Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                if (cookie == "UniversalId")
                {
                    System.Web.HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }
                else if (cookie == "ApiToken")
                {
                    System.Web.HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }
                else if (cookie == "PlantId")
                {
                    System.Web.HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }
                else if (cookie == "UserId")
                {
                    System.Web.HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }

            }
        }

        public int CheckCookies()

        {

            int i = 0;
            string[] myCookies = System.Web.HttpContext.Current.Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                if (cookie == "UniversalId")
                {
                    i++;
                }
                else if (cookie == "ApiToken")
                {
                    i++;
                }
                else if (cookie == "PlantId")
                {
                    i++;
                }
                else if (cookie == "UserId")
                {
                    i++;
                }

            }
            return i;
        }


        [ChildActionOnly]
        public string Upload(HttpPostedFileBase file, string fname)
        {
            string newfileName = "";
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(fileName);
                newfileName = fname + extension;
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedFile"), newfileName);
                file.SaveAs(path);
            }
            return newfileName;
        }
        public string saveImage(string base64String)
        {
            string filename = Guid.NewGuid().ToString() + ".jpg";
            System.Drawing.Image ImgFile = Base64ToImage(base64String);
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedFile/"), filename);
            ImgFile.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            return filename;
        }
        //File Upload End


        //Remote Post Start 
    }
}