using Microsoft.AspNet.Identity;
using Project_3.Helpers;
using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_3.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        // GET: Admin
        public ActionResult ManageRole()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "NameWithEmail");
            ViewBag.Role = new SelectList(db.Roles, "Name", "Name");
            var users = new List<ManageRoleViewModel>();
            foreach (var user in db.Users.ToList())
            {
                users.Add(new ManageRoleViewModel
                {
                    AvatarPath = $"{user.AvatarPath}",
                    FirstName = $"{user.FirstName}",
                    LastName = $"{user.LastName}",
                    Email = $"{user.Email}",
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault()

                });

            }

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRole(List<string> userIds, string role)
        {
            if (ModelState.IsValid)
            { 

                foreach (var userId in userIds)
            {
                var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                if (userRole != null)
                {
                    roleHelper.RemoveUserFromRole(userId, userRole);
                }
            }
            if (!string.IsNullOrEmpty(role))
            {
                foreach (var userId in userIds)
                {
                    roleHelper.AddUserToRole(userId, role);
                }

            }
            }
            return RedirectToAction("ManageRole", "Admin");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignRole(List<string> userIds)
        {
            if (ModelState.IsValid)
            { 
                    foreach (var userId in userIds)
                    {
                    var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                        if (userRole != null)
                        {
                            roleHelper.RemoveUserFromRole(userId, userRole);
                        }
                    }
            }

            return RedirectToAction("ManageRole", "Admin");
        }
       
        //manage project users get
        [Authorize(Roles = "Admin, DemoAdmin")]
        public ActionResult ManageProjectUsers()
        {
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "ProjectName");
            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "NameWithEmail");
            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "NameWithEmail");

            if (User.IsInRole("Admin")|| User.IsInRole("DemoAdmin"))
            {
                ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "NameWithEmail");
            }
            var myData = new List<ManageProjectViewModel>();           
            foreach (var user in db.Users.ToList())
            {
                var userVm = new ManageProjectViewModel
                {
                    NameWithEmail = $"{user.NameWithEmail}",
                    AvatarPath = $"{user.AvatarPath}",
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault(),
                    ProjectNames = user.Projects.Select(p => p.ProjectName).ToList()
                };
                if (userVm.ProjectNames.Count() == 0)

                    userVm.ProjectNames.Add("N/A");
                myData.Add(userVm);
            }
            return View(myData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(List<int> projects, string projectManagerId, List<string> developers, List<string> submitters)
        {
            if (projects != null)
            {
                foreach (var projectId in projects)

                {
                    foreach (var user in projectHelper.UsersOnProject(projectId).ToList())
                    {
                        projectHelper.RemoveUserFromProject(user.Id, projectId);

                    }
                    if (!string.IsNullOrEmpty(projectManagerId))
                    {
                        projectHelper.AddUserToProject(projectManagerId, projectId);

                    }
              
                    if (developers != null)
                    {
                        foreach(var developerId in developers)
                        {
                           projectHelper.AddUserToProject(developerId, projectId);
                        }

                        if (submitters != null)
                        {
                            foreach (var submitterId in submitters)
                            {
                                projectHelper.AddUserToProject(submitterId, projectId);
                            }

                        }
                    }
                    
                }
               
            }
            return RedirectToAction("ManageProjectUsers");
        }

    }
}



