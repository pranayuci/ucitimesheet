using DAL;
using Models.BO;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Implementations
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        private DataContext _dataContext;
        public EmployeeRepository(DataContext dataContext) : base(dataContext, true)
        {
            _dataContext = dataContext;
        }
    }
}
