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
        private TicketHelper ticketHelper = new TicketHelper();
        public JsonResult BarChartData()
        {
            
            var myData = new List<BarChart>();
            BarChart data = null;
            foreach (var priority in db.TicketPriorities.ToList())
            {
                var tickets = ticketHelper.ListMyTickets();
                data = new BarChart();
                data.label = priority.PriorityName;
                data.value = tickets.Where(t => t.TicketPriority.PriorityName == priority.PriorityName).Count();
                myData.Add(data);
            }

            return Json(myData);
        }

        public JsonResult PieChartData()
        {

            var colors = new string[6]{ "#ef1325", "#db30d0", "#0f1dbb", "#29ddcb", "#f1a71d", "#1db427" };
            var tickets = ticketHelper.ListMyTickets();         
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
  
            var myTickets = ticketHelper.ListMyTickets();
            var totalTicketCount = ticketHelper.ListMyTickets().Count();
            List<TicketTypeLiquidChartViewModel> resultList = myTickets.GroupBy(t => t.TicketType.TypeName).Select(g => new TicketTypeLiquidChartViewModel
            {
                TypeName = g.Key,
                Percentage = ((double)g.Count() / totalTicketCount).ToString("0.00")
            }).ToList();

            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
    }


}






