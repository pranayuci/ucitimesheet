using Models.BO.ViewModels;
using System;
using System.Linq;

namespace Repository.Contracts
{
    public interface ISPTotalHoursConsultantWise : IRepositoryBase<TotalHoursConsultantWiseVm>
    {
        //IQueryable<TotalHoursConsultantWiseVm> GetTotalHoursConsultantWise(string query, params object[] parameters);
        IQueryable<TotalHoursConsultantWiseVm> GetTotalHoursConsultantWise(DateTime starDate, DateTime endDate);

    }


}