using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

        //navigation
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser>Users {get; set;}

        //Constructor
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
      
    }
}