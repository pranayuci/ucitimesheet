using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Models.BO;
using Models.ViewModels;
using BusinessServices.Contracts;
using Microsoft.AspNetCore.Identity;

namespace WebAppCore.Pages.Roles
{
    public class CreateModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        [BindProperty]
        public CreateRoleVm CreateRoleVm { get; set; }

        public CreateModel(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public IActionResult OnGet()
        {
            CreateRoleVm = new CreateRoleVm();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await this.serviceFactory.RolesService.Create(CreateRoleVm);

            if ((result != null) && result.Succeeded)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }
        }
    }
}