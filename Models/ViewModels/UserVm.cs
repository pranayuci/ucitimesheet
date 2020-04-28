using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models.BO
{
    public class UserVm
    {
        public string Id { get; set; }
        public string Name { get; set; }      
        public string ClientName { get; set; }
        public string Department { get; set; }
        public string ProjectName { get; set; }
        public DateTime? ContractStartTime { get; set; }
        public DateTime? ContractEndTime { get; set; }
    }
}
