using Models.BO;
using Models.ViewModels;
using System;
using System.Collections.Generic;

namespace BusinessServices.Contracts
{
    public interface ITimesheetEntryService<T>
    {
        void CreateRange(IEnumerable<TimesheetVm> timesheetEntries);
        void UpdateRange(IEnumerable<TimesheetVm> timesheetEntries);

        IEnumerable<TimesheetVm> GetTimesheetsByUserId(string timesheetId);
        IEnumerable<TimesheetVm> GetUnApprovedTimesheetForWeek(DateTime startDate, DateTime endDate, string userId);
        IEnumerable<TimesheetVm> GetUnApprovedSubmittedTSForWeek(DateTime startDate, DateTime endDate, string userId);
        IEnumerable<TimesheetVm> GetTSForWeekForUser(DateTime startDate, DateTime endDate, string userId);
    }
}
