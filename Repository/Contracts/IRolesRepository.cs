using Microsoft.AspNetCore.Identity;
using Models.BO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Contracts
{
    public interface IRolesRepository : IRepositoryBase<IdentityRole>
    {
    }
}
