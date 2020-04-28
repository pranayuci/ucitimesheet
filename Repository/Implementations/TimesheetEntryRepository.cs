using DAL;
using Microsoft.EntityFrameworkCore;
using Models.BO;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Implementations
{
    public class TimesheetEntryRepository : RepositoryBase<TimesheetEntry>, ITimesheetEntryRepository
    {
        private DataContext _dataContext;
        public TimesheetEntryRepository(DataContext dataContext) : base(dataContext, true)
        {
            _dataContext = dataContext;
        }

        public void CreateRange(IEnumerable<TimesheetEntry> timesheetEntries)
        {
            _dataContext.AddRange(timesheetEntries);
            _dataContext.SaveChanges();
        }

        public IEnumerable<TimesheetEntry> GetTimesheetsByUserId(string userId)
        {
            return _dataContext.Set<TimesheetEntry>().AsNoTracking().Where(ts => ts.isApproved == false && ts.SubmittedBy == userId);
        }

        public IEnumerable<TimesheetEntry> GetTSForWeekForUser(DateTime startDate, DateTime endDate, string userId)
        {
            return _dataContext.Set<TimesheetEntry>()
                .AsNoTracking()
                .Where(ts => ts.Date >= startDate)
                .Where(ts => ts.Date <= endDate)
                .Where(ts => ts.SubmittedBy == userId)
                .ToList();
        }

        public IEnumerable<TimesheetEntry> GetUnApprovedTimesheetForWeek(DateTime startDate, DateTime endDate, string userId)
        {
            return _dataContext.Set<TimesheetEntry>()
                .AsNoTracking()
                .Where(ts => ts.Date >= startDate)
                .Where(ts => ts.Date <= endDate)
                //.Where(ts => ts.isApproved == false && ts.isSubmitted == true && ts.SubmittedBy == userId)
                .Where(ts => ts.isApproved == false && ts.SubmittedBy == userId)
                .ToList();
        }

        public IEnumerable<TimesheetEntry> GetUnApprovedSubmittedTSForWeek(DateTime startDate, DateTime endDate, string userId)
        {
            return _dataContext.Set<TimesheetEntry>()
                 .AsNoTracking()
                .Where(ts => ts.Date >= startDate)
                .Where(ts => ts.Date <= endDate)
                .Where(ts => ts.isApproved == false && ts.isSaveDraft == false && ts.isSubmitted == true && ts.SubmittedBy == userId)
                .ToList();
        }

        public void UpdateRange(IEnumerable<TimesheetEntry> timesheetEntries)
        {
            _dataContext.UpdateRange(timesheetEntries);
            _dataContext.SaveChanges();
        }
    }
}
