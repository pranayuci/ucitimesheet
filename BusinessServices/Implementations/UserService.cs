using BusinessServices.Contracts;
using Microsoft.AspNetCore.Identity;
using Models.BO;
using Models.ViewModels;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessServices.Implementations
{
    public class UserService : IUserService<User>
    {
        private IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> userManager;

        public UserService(IRepositoryWrapper repositoryWrapper, UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            this.userManager = userManager;
        }

        public async Task<IdentityResult> Create(ConsultantRegisterModelVm model)
        {
            try
            {
                var user = await userManager.FindByNameAsync(model.Email);

                //if (user == null)
                //{
                    user = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Email,
                        Email = model.Email,
                        Client = model.Client,
                        Department = model.Department,
                        ContractStartTime = model.ContractStartTime,
                        ContractEndTime = model.ContractEndTime,
                        ProjectName = model.ProjectName
                    };

                    return await userManager.CreateAsync(user, model.Password);
                //}
                //return null;
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

        public IEnumerable<User> GetAll()
        {
            return this.userManager.Users;
        }

        public async Task<User> GetById(string entityId)
        {
            return await this.userManager.FindByIdAsync(entityId);
        }

        public async Task<IdentityResult> Update(User entity)
        {
            try
            {
                return await this.userManager.UpdateAsync(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}