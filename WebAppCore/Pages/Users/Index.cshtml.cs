using BusinessServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAppCore.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IServiceFactory serviceFactory;
        public IEnumerable<User> Users { get; set; }

        public IndexModel(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public IActionResult OnGet()
        {
            Users = this.serviceFactory.UserService.GetAll().ToList();
            GetUserInfo();
            return Page();
        }

        private void GetUserInfo()
        {
            Users.ToList().ForEach(u =>
            {
                var client = this.serviceFactory.ClientService.GetById(Guid.Parse(u.Client));
                u.Client = client.Name;
            });
        }
    }
}