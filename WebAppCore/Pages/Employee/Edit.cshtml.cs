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

namespace WebAppCore.Pages.Employee
{
    public class EditModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        [BindProperty]
        public EmployeeVm Employee { get; set; }

        public EditModel(IServiceFactory serviceFactory)
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

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Employee = this.serviceFactory.EmployeeService.Update(Employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(Employee.EmployeeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EmployeeExists(Guid id)
        {
            return this.serviceFactory.EmployeeService.GetAll().ToList().Any(e => e.EmployeeId == id);
        }
    }
}
