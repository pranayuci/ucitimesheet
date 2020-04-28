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
    public class ForgotPasswordModel : PageModel
    {

        private readonly IServiceFactory serviceFactory;
        private readonly UserManager<User> userManager;
        private readonly IUserClaimsPrincipalFactory<User> claimsPrincipalFactory;

        [BindProperty]
        public ForgotPasswordVm ForgotPasswordVm { get; set; }

        public ForgotPasswordModel(IServiceFactory serviceFactory, UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> claimsPrincipalFactory)
        {
            this.serviceFactory = serviceFactory;
            this.userManager = userManager;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            ForgotPasswordVm = new ForgotPasswordVm();
            return Page();
        }

        public async Task<IActionResult> OnPost(ForgotPasswordVm forgotPasswordVm)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(forgotPasswordVm.Email);

                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    //var token = await userManager.generate(user);
                    //var resetUrl = Url.Action("ResetPassword", "Home",
                    //    new { token = token, email = user.Email }, Request.Scheme);
                    //string url = "http://localhost:44373/Account/ResetPassword?token=" + token + "&email=" + user.Email;
                    //return RedirectToPage(url);
                    //var resetUrl = Url.Page("./ResetPassword", new { token = token, email = user.Email });
                    var resetUrl = Url.Page("./ResetPassword");
                    TempData["token"] = token;
                    TempData["email"] = user.Email;

                    return Redirect(resetUrl);
                    //return RedirectToPage("./ResetPassword", new { token = token, email = user.Email });
                    //System.IO.File.WriteAllText("resetLink.txt", resetUrl);
                }
                else
                {
                    // email user and inform them that they do not have an account
                }

                //return RedirectToPage("./ResetPassword");
            }
            //return View();

            return Page();
        }
    }
}