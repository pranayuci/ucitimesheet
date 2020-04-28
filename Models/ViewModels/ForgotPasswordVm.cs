using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ForgotPasswordVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
