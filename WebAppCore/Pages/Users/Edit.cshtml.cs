using BusinessServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.BO;
using Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        private readonly UserManager<User> userManager;

        [BindProperty]
        public ConsultantRegisterModelVm ConsultantRegisterModelVm { get; set; }

        public List<ClientVm> Clients { get; set; }

        public EditModel(IServiceFactory serviceFactory, UserManager<User> userManager)
        {
            this.serviceFactory = serviceFactory;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string userId)
        {
            if (userId == null)
            {
                return RedirectToPage("./NotFound");
            }

            var user = await this.serviceFactory.UserService.GetById(userId);

            if (user == null)
            {
                return RedirectToPage("./NotFound");
            }

            Clients = this.serviceFactory.ClientService.GetAll().ToList();

            ConsultantRegisterModelVm = new ConsultantRegisterModelVm();

            ConsultantRegisterModelVm.Id = user.Id;
            ConsultantRegisterModelVm.FirstName = user.FirstName;
            ConsultantRegisterModelVm.LastName = user.LastName;
            ConsultantRegisterModelVm.Client = user.Client;
            ConsultantRegisterModelVm.Department = user.Department;
            ConsultantRegisterModelVm.ProjectName = user.ProjectName;
            ConsultantRegisterModelVm.ContractStartTime = user.ContractStartTime;
            ConsultantRegisterModelVm.ContractEndTime = user.ContractEndTime;
            ConsultantRegisterModelVm.Email = user.Email;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ConsultantRegisterModelVm == null)
            {
                return RedirectToPage("./NotFound");
            }

            var user = await this.serviceFactory.UserService.GetById(ConsultantRegisterModelVm.Id);

            if (user == null)
            {
                return RedirectToPage("./NotFound");
            }
            else
            {
                user.FirstName = ConsultantRegisterModelVm.FirstName;
                user.LastName = ConsultantRegisterModelVm.LastName;
                user.Client = ConsultantRegisterModelVm.Client;
                user.Department = ConsultantRegisterModelVm.Department;
                user.ProjectName = ConsultantRegisterModelVm.ProjectName;
                user.ContractStartTime = ConsultantRegisterModelVm.ContractStartTime;
                user.ContractEndTime = ConsultantRegisterModelVm.ContractEndTime;
                user.Email = ConsultantRegisterModelVm.Email;
                user.UserName = ConsultantRegisterModelVm.Email;

                var result = await this.serviceFactory.UserService.Update(user);

                if ((result != null) && result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }
            }

            return Page();
        }       
    }
}