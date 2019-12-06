using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Helpers
{
    public class UserHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private  ApplicationDbContext db2 = new ApplicationDbContext();
       
        public string GetAvatarPath()
        {
            return db2.Users.Find(HttpContext.Current.User.Identity.GetUserId()).AvatarPath;
        }

        public static string GetUserEmail()
        {
            return db.Users.Find(HttpContext.Current.User.Identity.GetUserId()).UserName;
        }
        public string GetUserFirstName()
        {
            return db2.Users.Find(HttpContext.Current.User.Identity.GetUserId()).FirstName;
        }
        public  string GetUserLastName()
        {
            return db2.Users.Find(HttpContext.Current.User.Identity.GetUserId()).LastName;
        }
        public static ApplicationUser GetUserId()
        {
            var userId = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            return userId;

        }
    }
}