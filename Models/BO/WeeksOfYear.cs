using System;
using System.ComponentModel.DataAnnotations;

namespace Models.BO
{
    public class WeeksOfYear
    {
        [Key]
        public int Id { get; set; }

        public int Year { get; set; }
        public int WeekNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}