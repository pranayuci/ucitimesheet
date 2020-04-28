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
    public class LoginModel : PageModel
    {

        private readonly IServiceFactory serviceFactory;
        private readonly UserManager<User> userManager;
        private readonly IUserClaimsPrincipalFactory<User> claimsPrincipalFactory;

        [BindProperty]
        public LoginModelVm LoginModelVm { get; set; }

        public LoginModel(IServiceFactory serviceFactory, UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> claimsPrincipalFactory)
        {
            this.serviceFactory = serviceFactory;
            this.userManager = userManager;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            LoginModelVm = new LoginModelVm();
            return Page();
        }

        public async Task<IActionResult> OnPost(LoginModelVm loginModelVm)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginModelVm.UserName);

                if (user != null && !await userManager.IsLockedOutAsync(user))
                {
                    if (await userManager.CheckPasswordAsync(user, loginModelVm.Password))
                    {
                        //if (!await userManager.IsEmailConfirmedAsync(user))
                        //{
                        //    ModelState.AddModelError("", "Email is not confirmed");
                        //    return View();
                        //}

                        //await userManager.ResetAccessFailedCountAsync(user);

                        //if (await userManager.GetTwoFactorEnabledAsync(user))
                        //{
                        //    var validProviders =
                        //        await userManager.GetValidTwoFactorProvidersAsync(user);

                        //    if (validProviders.Contains(userManager.Options.Tokens.AuthenticatorTokenProvider))
                        //    {
                        //        await HttpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme,
                        //            Store2FA(user.Id, userManager.Options.Tokens.AuthenticatorTokenProvider));
                        //        return RedirectToAction("TwoFactor");
                        //    }

                        //    if (validProviders.Contains("Email"))
                        //    {
                        //        var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
                        //        System.IO.File.WriteAllText("email2sv.txt", token);

                        //        await HttpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme,
                        //            Store2FA(user.Id, "Email"));
                        //        return RedirectToAction("TwoFactor");
                        //    }
                        //}

                        var principal = await claimsPrincipalFactory.CreateAsync(user);

                        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

                        return RedirectToPage("../Index");
                    }

                    await userManager.AccessFailedAsync(user);

                    //if (await userManager.IsLockedOutAsync(user))
                    //{
                    //    // email user, notifying them of lockout
                    //}
                }

                ModelState.AddModelError("", "Invalid UserName or Password");
            }

            return Page();
        }
    }
}