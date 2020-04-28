using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessServices.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.BO;
using Models.ViewModels;

namespace WebAppCore.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {

        private readonly IServiceFactory serviceFactory;
        private readonly UserManager<User> userManager;
        private readonly IUserClaimsPrincipalFactory<User> claimsPrincipalFactory;

        [BindProperty]
        public ResetPasswordVm ResetPasswordVm { get; set; }

        [BindProperty]
        public string Token { get; set; }

        [BindProperty]
        public string Email { get; set; }

        public ResetPasswordModel(IServiceFactory serviceFactory, UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> claimsPrincipalFactory)
        {
            this.serviceFactory = serviceFactory;
            this.userManager = userManager;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            Email = TempData["email"].ToString();
            Token = TempData["token"].ToString();

            ResetPasswordVm = new ResetPasswordVm();
            ResetPasswordVm.Token = Token;
            ResetPasswordVm.Email = Email;

            return Page();
        }

        public async Task<IActionResult> OnPost(ResetPasswordVm resetPasswordVm)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(resetPasswordVm.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, resetPasswordVm.Token, resetPasswordVm.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return Page();
                    }

                    //if (await userManager.IsLockedOutAsync(user))
                    //{
                    //    await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                    //}
                    //return View("Success");
                    RedirectToPage("./Success");
                }
                //ModelState.AddModelError("", "Invalid Request");
            }
            //return View();

            return Page();
        }
    }
}