using Models.BO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Contracts
{
    public interface ITimesheetEntryRepository : IRepositoryBase<TimesheetEntry>
    {
        void CreateRange(IEnumerable<TimesheetEntry> timesheetEntries);
        void UpdateRange(IEnumerable<TimesheetEntry> timesheetEntries);
        IEnumerable<TimesheetEntry> GetTimesheetsByUserId(string timesheetId);
        IEnumerable<TimesheetEntry> GetUnApprovedTimesheetForWeek(DateTime startDate, DateTime endDate, string userId);
        IEnumerable<TimesheetEntry> GetUnApprovedSubmittedTSForWeek(DateTime startDate, DateTime endDate, string userId);
        IEnumerable<TimesheetEntry> GetTSForWeekForUser(DateTime startDate, DateTime endDate, string userId);
    }
}
