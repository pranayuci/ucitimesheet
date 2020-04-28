using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Models.BO;
using Models.BO.ViewModels;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Implementations
{
    public class SPTotalHoursConsultantWiseRepo : RepositoryBase<TotalHoursConsultantWiseVm>, ISPTotalHoursConsultantWise
    {
        private DataContext _dataContext;
        public SPTotalHoursConsultantWiseRepo(DataContext dataContext) : base(dataContext, false)
        {
            _dataContext = dataContext;
        }

        //public IQueryable<TotalHoursConsultantWiseVm> GetTotalHoursConsultantWise(string query, params object[] parameters)
        public IQueryable<TotalHoursConsultantWiseVm> GetTotalHoursConsultantWise(DateTime starDate, DateTime endDate)
        {
            IQueryable<TotalHoursConsultantWiseVm> results;

            //try
            //{
            //    if (parameters != null)
            //    {
            //        results = this._dataContext.Query<TotalHoursConsultantWiseVm>().FromSql(query, parameters);
            //    }
            //    else
            //    {
            //        results = this._dataContext.Query<TotalHoursConsultantWiseVm>().FromSql(query);
            //    }

            //    return results;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}

            try
            {
                //this._dataContext.Set<TimesheetEntry>().Join(this._dataContext.Set<User>(), ts => ts.SubmittedBy, ur => ur.Id,
                //    (ts, ur) => new TotalHoursConsultantWiseVm
                //    {
                //        Id = ur.Id,
                //        FirstName = ur.FirstName,
                //        LastName = ur.LastName
                //    });

                //results =  this._dataContext.Set<TimesheetEntry>().Join(this._dataContext.Set<User>(), ts => ts.SubmittedBy, ur => ur.Id,
                //    (ts, ur) => new { ts, ur })
                //    .Where(c => c.ts.Date >= DateTime.Now && c.ts.Date <= DateTime.Now.AddDays(7))
                //    .GroupBy(x => new { x.ur.Id, x.ur.FirstName, x.ur.LastName, x.ts.WorkHours })
                //    .Select(g => new TotalHoursConsultantWiseVm
                //    {
                //        Id = g.Key.Id,
                //        FirstName = g.Key.FirstName,
                //        LastName = g.Key.LastName,
                //        TotalHours = g.Count()
                //    });

                //results = this._dataContext.Set<TimesheetEntry>()
                //    .Join(this._dataContext.Set<User>(), ts => ts.SubmittedBy, ur => ur.Id, (ts, ur) => new { ts, ur })
                //      .Where(c => c.ts.Date >= starDate && c.ts.Date <= endDate)
                //      .GroupBy(x => new
                //      {
                //          x.ur.Id,
                //          x.ur.FirstName,
                //          x.ur.LastName
                //      })
                //      .Select(g => new TotalHoursConsultantWiseVm
                //      {
                //          Id = g.Key.Id,
                //          FirstName = g.Key.FirstName,
                //          LastName = g.Key.LastName,
                //          TotalHours = g.Sum(s => s.ts.WorkHours)
                //      });

                results = this._dataContext.Set<TimesheetEntry>()
                .Join(this._dataContext.Set<User>(), ts => ts.SubmittedBy, ur => ur.Id, (ts, ur) => new { ts, ur })
                .Join(this._dataContext.Set<Client>(), first => first.ur.Client, second => second.ClientId.ToString(),
                            (first, second) => new { first, second })
                             .Where(c => c.first.ts.Date >= starDate && c.first.ts.Date <= endDate)
                   .GroupBy(x => new
                   {
                       x.first.ur.Id,
                       x.first.ur.FirstName,
                       x.first.ur.LastName,
                       x.first.ur.ProjectName,
                       x.second.Name,
                       x.first.ur.Client
                   })
                   .Select(g => new TotalHoursConsultantWiseVm
                   {
                       Id = g.Key.Id,
                       FirstName = g.Key.FirstName,
                       LastName = g.Key.LastName,
                       ClientName = g.Key.Name,
                       ProjectName = g.Key.ProjectName,
                       ClientId = g.Key.Client,
                       TotalHours = g.Sum(s => s.first.ts.WorkHours)
                   });


                //results = this._dataContext.Set<TimesheetEntry>()
                // .Join(this._dataContext.Set<User>(), ts => ts.SubmittedBy, ur => ur.Id, (ts, ur) => new { ts, ur })
                //   .Where(c => c.ts.Date >= starDate && c.ts.Date <= endDate)
                //   .GroupBy(x => new
                //   {
                //       x.ur.Id,
                //       x.ur.FirstName,
                //       x.ur.LastName
                //   })
                //   .Select(g => new TotalHoursConsultantWiseVm
                //   {
                //       Id = g.Key.Id,
                //       FirstName = g.Key.FirstName,
                //       LastName = g.Key.LastName,
                //       TotalHours = g.Sum(s => s.ts.WorkHours)
                //   });



                return results;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
