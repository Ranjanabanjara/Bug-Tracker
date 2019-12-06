using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project_3.Helpers;
using Project_3.Models;


namespace Project_3.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        public ActionResult ManageUsers(int id)
        {
            ViewBag.ProjectId = id;
            var pmId = projectHelper.ListUsersOnProjectInRole(id, "ProjectManager").FirstOrDefault();
            ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "NameWithEmail", pmId);
            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "NameWithEmail", projectHelper.ListUsersOnProjectInRole(id, "Developers"));
            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "NameWithEmail", projectHelper.ListUsersOnProjectInRole(id, "Submitters"));

            return View();
 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUsers(int projectId, string projectManagerId, List<string>developers, List<string>submitters)
        {
            foreach(var user in projectHelper.UsersOnProject(projectId).ToList())
            {
                projectHelper.RemoveUserFromProject(user.Id, projectId);

            }
            if (!string.IsNullOrEmpty(projectManagerId))
            {
                projectHelper.AddUserToProject(projectManagerId, projectId);
            }
            if(developers != null)
            {
                foreach(var developerId in developers)
                {
                    projectHelper.AddUserToProject(developerId, projectId);

                }
            }
            if(submitters != null)
            {
                foreach(var submitterId in submitters)
                {
                    projectHelper.AddUserToProject(submitterId, projectId);
                }

            }
           
            return RedirectToAction("Index", "Projects");
        }
            
        // GET: Projects
        
        public ActionResult Index(string badProj)
        {
            List<Project> myProjects = new List<Project>();
            if (!string.IsNullOrEmpty(badProj))
            {
                return View(projectHelper.ProjectsMissingRoles().ToList());
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var userRole = roleHelper.ListUserRoles(user.Id).FirstOrDefault();
                if (userRole == "DemoAdmin" || userRole == "Admin")
                {
                    return View(db.Projects.ToList());
                }

                return View(projectHelper.ListUserProjects(userId).ToList());

            }
          
        }


        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        [Authorize(Roles = "DemoAdmin")]
        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectName,Description,Created")] Project project)
        {
            if (ModelState.IsValid)
            {

                project.Created = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectName,Description,Created")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
