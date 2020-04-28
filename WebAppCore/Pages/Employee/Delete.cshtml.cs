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
    public class DeleteModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        [BindProperty]
        public EmployeeVm Employee { get; set; }

        public DeleteModel(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }
       

        public IActionResult OnGet(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = this.serviceFactory.EmployeeService.GetById(id);

            if (Employee == null)
            {
                return NotFound();
            }
            return Page();
        }

        public  IActionResult OnPost(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = this.serviceFactory.EmployeeService.GetById(id);

            if (Employee != null)
            {
                Employee = this.serviceFactory.EmployeeService.Delete(Employee.EmployeeId);
            }

            return RedirectToPage("./Index");
        }
    }
}
