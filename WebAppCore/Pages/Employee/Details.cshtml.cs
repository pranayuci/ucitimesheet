using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Models.BO;
using BusinessServices.Contracts;
using Models.ViewModels;

namespace WebAppCore.Pages.Employee
{
    public class DetailsModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        [BindProperty]
        public EmployeeVm Employee { get; set; }

        public DetailsModel(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }


        public IActionResult OnGet(Guid employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }

            Employee = this.serviceFactory.EmployeeService.GetById(employeeId);

            if (Employee == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
