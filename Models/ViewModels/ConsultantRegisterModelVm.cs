using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ConsultantRegisterModelVm
    {
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Client { get; set; }
        public string Department { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public DateTime? ContractStartTime { get; set; }
        [Required]
        public DateTime? ContractEndTime { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
