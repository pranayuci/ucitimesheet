using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models.BO
{
    public class Timesheet
    {
        public Guid TimesheetId { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public string Name { get; set; }        
    }
}
