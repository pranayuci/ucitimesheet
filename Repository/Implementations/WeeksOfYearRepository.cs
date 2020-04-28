using DAL;
using Models.BO;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Implementations
{
    public class WeeksOfYearRepository : RepositoryBase<WeeksOfYear>, IWeeksOfYearRepository
    {
        private DataContext _dataContext;
        public WeeksOfYearRepository(DataContext dataContext) : base(dataContext, true)
        {
            _dataContext = dataContext;
        }

        public WeeksOfYear GetWeekByDate(DateTime dateTime)
        {
            return _dataContext.Set<WeeksOfYear>().Where(wy => wy.StartDate <= dateTime).Where(wy => wy.EndDate >= dateTime).SingleOrDefault();
        }

        public WeeksOfYear GetWeekByWeekNumber(int weekNumber)
        {
            return _dataContext.Set<WeeksOfYear>().Where(wy => wy.WeekNumber == weekNumber).SingleOrDefault();

        }
    }
}
