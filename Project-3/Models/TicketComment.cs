﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
       
        //navigation
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set;}


    }

}