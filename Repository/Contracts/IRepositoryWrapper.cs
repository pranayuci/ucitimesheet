using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Contracts
{
    public interface IRepositoryWrapper
    {
        IEmployeeRepository EmployeeRepository { get; }
        IRolesRepository RolesRepository { get; }
        ITimesheetEntryRepository TimesheetEntryRepository { get; }
        IClientRepository ClientRepository { get; }
        IWeeksOfYearRepository WeeksOfYearRepository { get; }
        ISPTotalHoursConsultantWise SPTotalHoursConsultantWise { get; }
    }
}
