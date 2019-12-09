using Microsoft.AspNet.Identity;
using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project_3.Helpers
{
    public class TicketHistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        public void RecordHistoricalChanges(Ticket oldTicket, Ticket newTicket)
        {
            if(oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "TicketStatusId",
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.TicketStatus.StatusName,
                    NewValue = newTicket.TicketStatus.StatusName,
                    Changed = (DateTime)newTicket.Updated

                };
                db.TicketHistories.Add(ticketHistory);

            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "TicketPriorityId",
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.TicketPriority.PriorityName,
                    NewValue = newTicket.TicketPriority.PriorityName,
                    Changed = (DateTime)newTicket.Updated

                };
                db.TicketHistories.Add(ticketHistory);

            }
            if (oldTicket.Title != newTicket.Title)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "Title",
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    Changed = (DateTime)newTicket.Updated

                };
                db.TicketHistories.Add(ticketHistory);

            }
            if (oldTicket.Description != newTicket.Description)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "Description",
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.Description,
                    NewValue = newTicket.Description,
                    Changed = (DateTime)newTicket.Updated

                };
                db.TicketHistories.Add(ticketHistory);

            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "TicketTypeId",
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.TicketType.TypeName,
                    NewValue = newTicket.TicketType.TypeName,
                    Changed = (DateTime)newTicket.Updated

                };
                db.TicketHistories.Add(ticketHistory);

            }
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "AssignedToUserId",
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.AssignedToUser == null? "UnAssigned":oldTicket.AssignedToUser.FullName,
                    NewValue = newTicket.AssignedToUser == null? "UnAssigned" :newTicket.AssignedToUser.FullName,
                    Changed = (DateTime)newTicket.Updated

                };
                db.TicketHistories.Add(ticketHistory);

            }


            db.SaveChanges();

        }
        public  List<TicketHistory> TicketHistories()
        {
            var myTickets = ticketHelper.ListAllTickets();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var userRole = roleHelper.ListUserRoles(user.Id).FirstOrDefault();

            if (userRole == "DemoAdmin")
            {
                return db.TicketHistories.ToList();
            } else
            {
                return myTickets.SelectMany(t => t.TicketHistories).ToList();
            }

        }

    }
}