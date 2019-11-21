using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project_3.Helpers;
using Project_3.Models;

namespace Project_3.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create(int id)
        {
            var attachment = new TicketAttachment
            {
                TicketId = id
            };
           
            return View(attachment);

        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId,Descriptionn")] TicketAttachment ticketAttachment, HttpPostedFileBase file, string Descriptionn)
        {
            if (ModelState.IsValid)
            {
                
                if (file != null)
                {
                    if (UploadValidator.IsWebFriendlyFile(file) || UploadValidator.IsWebFriendlyImage(file))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var onlyFilename = Path.GetFileNameWithoutExtension(fileName);
                        onlyFilename = StringUtilities.URLFriendly(onlyFilename);
                        fileName = $"{onlyFilename}_{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
                        file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                        ticketAttachment.MediaUrl = "/Uploads/" + fileName;
                        ticketAttachment.Created = DateTime.Now;
                        ticketAttachment.UserId = User.Identity.GetUserId();
                        ticketAttachment.Description = Descriptionn;
                        db.TicketAttachments.Add(ticketAttachment);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId});
                //return View("Index");
            }

            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Description")] TicketAttachment ticketAttachment, int attachId)
        {
            if (ModelState.IsValid)
            {
                var newattach = db.TicketAttachments.Find(attachId);
                newattach.Description = ticketAttachment.Description;
                 db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = newattach.TicketId});
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? attachId)
        {
            if (attachId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(attachId);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? attachId, int TicketId)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(attachId);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Details", "Tickets", new { id = TicketId });
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
