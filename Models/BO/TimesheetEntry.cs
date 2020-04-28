using Models.BO;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models.BO
{
    public class TimesheetEntry
    {
        [Key]
        public Guid TimesheetEntryId { get; set; }
        public string Day { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public float WorkHours { get; set; }
        public float LunchHours { get; set; }
        public string Comments { get; set; }
        public bool isSubmitted { get; set; }
        public bool isSaveDraft { get; set; }
        public bool isRejected { get; set; }
        public DateTime SubmittedDate { get; set; }           
        public string SubmittedBy { get; set; }
        public string SubmittedFrom { get; set; } // later capture IP address
        public bool isApproved { get; set; }
        public DateTime ApprovedDate { get; set; }       
        public string ApprovedBy { get; set; }
        public string ApprovedFrom { get; set; } // later capture IP address
        public string CommentFromApprover { get; set; }
        public bool isDisabled { get; set; }
    }
}
