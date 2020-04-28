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
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace WebAppCore.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        public IEnumerable<IdentityRole> Roles { get; set; }


        public IndexModel(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }


        public IActionResult OnGet()
        {
            Roles = this.serviceFactory.RolesService.GetAll().ToList();
            return Page();
        }
    }
}
