using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Models
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

    }
    public class ManageRoleViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public string RoleName { get; set; }
    }
    public class ManageProjectViewModel
    {
       
        public string NameWithEmail { get; set; }
        public string AvatarPath { get; set; }
        public string RoleName { get; set; }
        public List<string> ProjectNames { get; set; }
        public ManageProjectViewModel()
        {
            ProjectNames = new List<string>();
        }
    }

    public class TicketTypeLiquidChartViewModel
    {
        public string TypeName { get; set; }
        public string Percentage { get; set; }
    }

}