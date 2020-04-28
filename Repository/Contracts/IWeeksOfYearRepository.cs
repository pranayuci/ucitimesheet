using Models.BO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Contracts
{
    public interface IWeeksOfYearRepository : IRepositoryBase<WeeksOfYear>
    {
        WeeksOfYear GetWeekByDate(DateTime dateTime);
        WeeksOfYear GetWeekByWeekNumber(int weekNumber);
    }
}
