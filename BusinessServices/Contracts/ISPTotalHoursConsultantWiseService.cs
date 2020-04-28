using Models.BO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessServices.Contracts
{
    public interface ISPTotalHoursConsultantWiseService<T>
    {
        IQueryable<TotalHoursConsultantWiseVm> GetTotalHoursConsultantWise(DateTime starDate, DateTime endDate);

    }
}
