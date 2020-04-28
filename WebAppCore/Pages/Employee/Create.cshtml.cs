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

namespace WebAppCore.Pages.Employee
{
    public class CreateModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        [BindProperty]
        public EmployeeVm Employee { get; set; }

        public CreateModel(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public IActionResult OnGet()
        {
            Employee = new EmployeeVm();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            Employee = this.serviceFactory.EmployeeService.Create(Employee);          

            return RedirectToPage("./Index");
        }
    }
}