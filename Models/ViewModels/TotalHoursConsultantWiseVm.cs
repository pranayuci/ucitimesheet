using System;
using System.ComponentModel.DataAnnotations;

namespace Models.BO.ViewModels
{
    public class TotalHoursConsultantWiseVm
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string ProjectName { get; set; }
        public float TotalHours { get; set; }

    }
}
