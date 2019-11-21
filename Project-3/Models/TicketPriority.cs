using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }
        public string PriorityName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public TicketPriority()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}