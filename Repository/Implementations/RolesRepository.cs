using DAL;
using Microsoft.AspNetCore.Identity;
using Models.BO;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Implementations
{
    public class RolesRepository : RepositoryBase<IdentityRole>, IRolesRepository
    {
        private DataContext _dataContext;
        public RolesRepository(DataContext dataContext) : base(dataContext, true)
        {
            _dataContext = dataContext;
        }
    }
}
