using Microsoft.AspNet.Identity;
using Project_3.Helpers;
using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();

        public JsonResult BarChartData()
        {
            var myData = new List<BarChart>();
            BarChart data = null;
            foreach (var priority in db.TicketPriorities.ToList())
            {
                data = new BarChart();
                data.label = priority.PriorityName;
                data.value = db.Tickets.Where(t => t.TicketPriority.PriorityName == priority.PriorityName).Count();
                myData.Add(data);
            }

            return Json(myData);
        }

        public JsonResult PieChartData()
        {

            var colors = new string[6]{ "blue", "red", "green", "yellow", "purple", "pink"};
            var tickets = db.Tickets.ToList();
            var pieData = new List<PieChart>();
            var index = 0;
            foreach(var priority in db.TicketPriorities.ToList())
            {
                pieData.Add(new PieChart
                {
                    label = priority.PriorityName,
                    data = tickets.Where(t => t.TicketPriority.PriorityName == priority.PriorityName).Count(),
                    color = colors[index]


                });
                index++ ;
 

            }
            


            return Json(pieData);
        }

        public JsonResult LiquidChartData()
        {
            var userId = User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var myTickets = new List<Ticket>();

            switch (myRole)
            {

                case "Developer":
                    myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    break;
                case "Submitter":
                    myTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
                case "ProjectManager":
                    //myTickets are going to be all the Tickets on all the Projects I am on.
                    myTickets = db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();
                    break;
                case "DemoAdmin":
                    myTickets = db.Tickets.ToList();
                    break;
            }

            var totalTicketCount = myTickets.Count();
            List<TicketTypeLiquidChartViewModel> resultList = myTickets.GroupBy(t => t.TicketType.TypeName).Select(g => new TicketTypeLiquidChartViewModel
            {
                TypeName = g.Key,
                Percentage = ((double)g.Count() / totalTicketCount).ToString("0.00")
            }).ToList();

            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
    }


}






