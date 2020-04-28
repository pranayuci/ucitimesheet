using DAL;
using Models.BO;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Implementations
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        private DataContext _dataContext;
        public ClientRepository(DataContext dataContext) : base(dataContext, true)
        {
            _dataContext = dataContext;
        }
    }
}
