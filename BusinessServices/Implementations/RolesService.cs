using AutoMapper;
using BusinessServices.Contracts;
using Microsoft.AspNetCore.Identity;
using Models.BO;
using Models.ViewModels;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Implementations
{
    public class RolesService : IRolesServices<CreateRoleVm>
    {
        IRepositoryWrapper _repositoryWrapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesService(IRepositoryWrapper repositoryWrapper, RoleManager<IdentityRole> roleManager)
        {
            _repositoryWrapper = repositoryWrapper;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> Create(CreateRoleVm entity)
        {
            try
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = entity.RoleName
                };
                return await this.roleManager.CreateAsync(identityRole);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public CreateRoleVm Delete(Guid entityId)
        {
            return null;
        }

        public IEnumerable<IdentityRole> GetAll()
        {
            return this.roleManager.Roles;
        }

        public async Task<IdentityRole> GetById(string entityId)
        {
            return await this.roleManager.FindByIdAsync(entityId);
        }

        public async Task<IdentityResult> Update(IdentityRole entity)
        {
            try
            {
                return await this.roleManager.UpdateAsync(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
