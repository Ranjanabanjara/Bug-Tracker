using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        //foreign key
        public int TicketId { get; set; }
        public string UserId { get; set; }
        
        public string Description { get; set; }
        public DateTime Created { get; set; }

        public string MediaUrl { get; set; }

        //navigation 
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}