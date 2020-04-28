using BusinessServices.Implementations;
using System;
using System.Collections.Generic;

namespace BusinessServices.Contracts
{
    public interface IServiceFactory
    {
        EmployeeService EmployeeService { get; }
        RolesService RolesService { get; }
        UserService UserService { get; }
        TimesheetEntryService TimesheetEntryService { get; }
        ClientService ClientService { get; }
        WeeksOfYearService WeeksOfYearService { get; }
        SPTotalHoursConsultantWiseService SPTotalHoursConsultantWiseService { get; }
        EmailService EmailService { get; }

    }
}
