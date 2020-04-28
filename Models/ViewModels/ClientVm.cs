using System;
using System.ComponentModel.DataAnnotations;

namespace Models.BO
{
    public class ClientVm
    {
        public Guid ClientId { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
