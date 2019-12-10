﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project_3.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }
     
        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }
        public List<string> ListUsersOnProjectInRole(int projectId, string roleName)
        {
            var userIdList = new List<string>();
            foreach (var user in UsersOnProject(projectId))
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                    userIdList.Add(user.Id);
            }
            return userIdList;
        }
        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);
                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified; // just saves this obj instance.
                db.SaveChanges();
            }
        }
        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }
        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }


        public ICollection<Project> ProjectsMissingRoles()
        {
            
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var unmanagedProjects = new List<Project>();
            var allProjects = db.Projects.ToList();

            if (roleHelper.IsUserInRole(userId, "DemoAdmin"))
            {
                foreach (var project in allProjects)
                {
                    if (ListUsersOnProjectInRole(project.Id, "ProjectManager").Count() == 0 ||
                       ListUsersOnProjectInRole(project.Id, "Developer").Count() == 0 ||
                       ListUsersOnProjectInRole(project.Id, "Submitter").Count() == 0)
                    {
                        unmanagedProjects.Add(project);
                    }
                }

            }
            if (roleHelper.IsUserInRole(userId, "ProjectManager"))
            {
                foreach (var project in user.Projects)
                {
                    if (ListUsersOnProjectInRole(project.Id, "Developer").Count() == 0 ||
                       ListUsersOnProjectInRole(project.Id, "Submitter").Count() == 0)

                    {
                        unmanagedProjects.Add(project);
                    }
                }

            }
          
            return unmanagedProjects;

        }

    }
}

