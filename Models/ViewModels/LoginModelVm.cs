using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class LoginModelVm
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
