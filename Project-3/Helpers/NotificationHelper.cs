using Microsoft.AspNet.Identity;
using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Helpers
{
    public class NotificationHelper
    {
     
        private static ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
           
            var ticketHasBeenAssigned = string.IsNullOrEmpty(oldTicket.AssignedToUserId);
            var ticketHasBeenUnAssigned = string.IsNullOrEmpty(newTicket.AssignedToUserId);
            
           

            if (ticketHasBeenAssigned)
                AddAssignmentNotification(oldTicket, newTicket);

            else if (ticketHasBeenUnAssigned)
                AddUnassignmentNotification(oldTicket, newTicket);
           
        }
    

        private void AddAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                IsRead = false,
                ReceipentId = newTicket.AssignedToUserId,
                Created = DateTime.Now,
                NotificationBody = $"You have been assigned to a Ticket: {newTicket.Title} created on: {newTicket.Created} on Project: {newTicket.Project.ProjectName}."

            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        private void AddUnassignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = oldTicket.Id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                IsRead = false,
                ReceipentId = oldTicket.AssignedToUserId,
                Created = DateTime.Now,
                NotificationBody = $"You have been unassigned from a Ticket {oldTicket.Title} created on: {oldTicket.Created} on Project: {oldTicket.Project.ProjectName}."

            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();


        }
    
        public static List<TicketNotification> GetUnreadNotifications() 
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Include("Sender").Where(t => t.ReceipentId == currentUserId && !t.IsRead).ToList();

        }

        public static List<TicketNotification> UsersAllNotification()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Where(t => t.ReceipentId == userId).ToList();
        }

     


    }
}