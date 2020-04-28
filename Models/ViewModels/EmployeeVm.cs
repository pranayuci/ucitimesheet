using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class EmployeeVm
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
