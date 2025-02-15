using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;
using Owin;
using ReportingAssistant.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
[assembly:OwinStartup(typeof(ReportingAssistant.Startup))]
namespace ReportingAssistant
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions() { AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie, LoginPath = new PathString("/Account/Login") });
            this.CreateRolesandUser();
        }
        public void CreateRolesandUser()
        {
            var ManageRole = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            var db = new ApplicationDbContext();
            var userAppUserStore = new ApplicationUserStore(db);
            var userManager = new ApplicationUserManager(userAppUserStore);
            if (!ManageRole.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                ManageRole.Create(role);//AspNetRole table.
            }

            if (!ManageRole.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                ManageRole.Create(role);
            }
            //Behind code is to create new admin user in database if data is not exist in database.

            if (userManager.FindByName("Harish") == null)
            {
                var user = new ApplicationUser();
                user.UserName = "Harish";
                user.Email = "harish@gmail.com";
                string password = "Hari123";
                var chckUser = userManager.Create(user, password);//AspNetUsers Table
                if (chckUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    userManager.AddToRole(user.Id, "User");
                }

            }

            if (userManager.FindByName("Shirisha") == null)
            {
                var user = new ApplicationUser();
                user.UserName = "Shirisha";
                user.Email = "Shirisha.test.com";
                string password = "Shirisha123";
                var chckUser = userManager.Create(user, password);
                if (chckUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
            if (userManager.FindByName("Ganesh") == null)
            {
                var user = new ApplicationUser();
                user.UserName = "Ganesh";
                user.Email = "Ganesh.test.com";
                string password = "Ganesh966";
                var chckUser = userManager.Create(user, password);
                if (chckUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }


            if (userManager.FindByName("Sainath") == null)
            {
                var user = new ApplicationUser();
                user.UserName = "Sainath";
                user.Email = "Sainath.test.com";
                string password = "Sainath123";
                var chckUser = userManager.Create(user, password);
                if (chckUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }




            //Behind code is to create new Manager user in database if data is not exist in database.

            //if (userManager.FindByName("harsha") == null)
            //{
            //    var user = new ApplicationUser();
            //    user.UserName = "Harsha";
            //    user.Email = "harsha.test.com";
            //    string password = "harsha123";
            //    var chckUser = userManager.Create(user, password);
            //    if (chckUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "Manager");
            //    }
            //}
            if (userManager.FindByName("Deepak") == null)
            {
                var user = new ApplicationUser();
                user.UserName = "Deepak";
                user.Email = "Deepak.test.com";
                string password = "Deepak123";
                var chckUser = userManager.Create(user, password);//AspNetUsers Table
                if (chckUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "User");
                }
            }
            if (userManager.FindByName("Rishika") == null)
            {
                var user = new ApplicationUser();
                user.UserName = "Rishika";
                user.Email = "Rishika.test.com";
                string password = "Rishika123";
                var chckUser = userManager.Create(user, password);//AspNetUsers Table
                if (chckUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "User");
                }
            }

            //Creating Customer Role in database

            //if (!ManageRole.RoleExists("User"))
            //{
            //    var role = new IdentityRole();
            //    role.Name = "User";
            //    ManageRole.Create(role);
            //}

            ////Behind code is to create new Customer user in database if data is not exist in database.

            //if (userManager.FindByName("User") == null)
            //{
            //    var user = new ApplicationUser();
            //    user.UserName = "User";
            //    user.Email = "User.task.com";
            //    string password = "user123";
            //    var chckUser = userManager.Create(user, password);
            //    if (chckUser.Succeeded)
            //    {
            //        userManager.AddToRole(user.Id, "User");
            //    }
            //}
        }
    }
}