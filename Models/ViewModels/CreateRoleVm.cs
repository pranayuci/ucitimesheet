using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class CreateRoleVm
    {
        [Required]
        public string RoleName { get; set; }
    }
}
