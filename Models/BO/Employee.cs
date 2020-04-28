using System;
using System.ComponentModel.DataAnnotations;

namespace Models.BO
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
