using BusinessServices.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.BO;
using Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppCore.Pages.Roles
{
    public class UserRoleAssignmentModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        private readonly UserManager<User> userManager;

        [BindProperty]
        public CreateRoleVm CreateRoleVm { get; set; }

        [BindProperty]
        public List<UserRoleVm> UserRoleVm { get; set; }

        [TempData]
        public string RoleId { get; set; }

        public UserRoleAssignmentModel(IServiceFactory serviceFactory, UserManager<User> userManager)
        {
            this.serviceFactory = serviceFactory;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string roleId)
        {
            RoleId = roleId;
            var role = await this.serviceFactory.RolesService.GetById(roleId);

            if (role == null)
            {
                return RedirectToPage("./NotFound");
            }

            UserRoleVm = new List<UserRoleVm>();
            foreach (var user in this.userManager.Users)
            {
                var userRoleVm = new UserRoleVm
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                userRoleVm.isSelected = await this.userManager.IsInRoleAsync(user, role.Name);
                UserRoleVm.Add(userRoleVm);
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(List<UserRoleVm> userRoleVm, string roleId)
        {
            var role = await this.serviceFactory.RolesService.GetById(roleId);

            if (role == null)
            {
                return RedirectToPage("./NotFound");
            }
            for (int i = 0; i < userRoleVm.Count; i++)
            {
                var user = await this.userManager.FindByIdAsync(userRoleVm[i].UserId);
                IdentityResult result;

                if (userRoleVm[i].isSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRoleVm[i].isSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < userRoleVm.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToPage("./Edit", new { roleId = roleId });
                    }
                }
            }

            return RedirectToPage("./Edit", new { roleId = roleId });
        }
    }
}