using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Models.BO;
using Models.ViewModels;
using BusinessServices.Contracts;
using Microsoft.AspNetCore.Identity;

namespace WebAppCore.Pages.Roles
{
    public class EditModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        private readonly UserManager<User> userManager;

        [BindProperty]
        public EditRoleVm EditRoleVm { get; set; }

        public EditModel(IServiceFactory serviceFactory, UserManager<User> userManager)
        {
            this.serviceFactory = serviceFactory;
            this.userManager = userManager;
        }



        public async Task<IActionResult> OnGet(string roleId)
        {
            if (roleId == null)
            {
                return RedirectToPage("./NotFound");
            }

            var role = await this.serviceFactory.RolesService.GetById(roleId);

            if (role == null)
            {
                return RedirectToPage("./NotFound");
            }

            EditRoleVm = new EditRoleVm();

            EditRoleVm.Id = role.Id;
            EditRoleVm.RoleName = role.Name;            

            foreach (var user in this.userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    EditRoleVm.Users.Add(user.UserName);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (EditRoleVm == null)
            {
                return RedirectToPage("./NotFound");
            }

            var role = await this.serviceFactory.RolesService.GetById(EditRoleVm.Id);

            if (role == null)
            {
                return RedirectToPage("./NotFound");
            }
            else
            {
                role.Name = EditRoleVm.RoleName;
                var result = await this.serviceFactory.RolesService.Update(role);
                if ((result != null) && result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }
            }

            return Page();
        }

        [HttpGet]
        public void OnGet1(string roleId) { }
    }
}
