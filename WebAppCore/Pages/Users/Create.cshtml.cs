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
using Microsoft.AspNetCore.Authorization;

namespace WebAppCore.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        [BindProperty]
        public ConsultantRegisterModelVm ConsultantRegisterModelVm { get; set; }

        public List<ClientVm> Clients { get; set; }

        public ClientVm SelectedClient { get; set; }

        public CreateModel(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public IActionResult OnGet()
        {
            Clients = this.serviceFactory.ClientService.GetAll().ToList();
            //SelectedClient = Clients[0];
            ConsultantRegisterModelVm = new ConsultantRegisterModelVm();
            
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Clients = this.serviceFactory.ClientService.GetAll().ToList();
            //SelectedClient = Clients[0];
            //ConsultantRegisterModelVm = new ConsultantRegisterModelVm();

            if (!ModelState.IsValid)
            {
                return Page();
            }
            //ConsultantRegisterModelVm.Client = SelectedClient.ClientId.ToString();
            var result = await this.serviceFactory.UserService.Create(ConsultantRegisterModelVm);

            if ((result != null) && result.Succeeded)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                if (result != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return Page();
            }
        }
    }
}