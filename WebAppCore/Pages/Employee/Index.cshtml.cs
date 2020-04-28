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
    public class IndexModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        public IList<EmployeeVm> Employee { get; set; }


        public IndexModel(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }


        public IActionResult OnGet()
        {
            Employee = this.serviceFactory.EmployeeService.GetAll().ToList();
            return Page();
        }
    }
}
