using Models.BO;
using System;
using System.Collections.Generic;

namespace BusinessServices.Contracts
{
    public interface IWeeksOfYearService<T>
    {
        WeeksOfYearVm GetWeekByDate(DateTime dateTime);
        WeeksOfYearVm GetWeekByWeekNumber(int weekNumber);

    }
}
