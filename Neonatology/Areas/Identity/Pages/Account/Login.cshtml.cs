﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace Neonatology.Areas.Identity.Pages.Account
{
#nullable disable

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Data.Models;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    using Services;

    using static Common.GlobalConstants;

    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginModel> logger;
        private readonly ReCaptchaService reCaptchaService;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, ReCaptchaService reCaptchaService)
        {
            this.signInManager = signInManager;
            this.logger = logger;
            this.reCaptchaService = reCaptchaService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = MessageConstants.RequiredFieldErrorMsg)]
            [Display(Name = "И-мейл")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = MessageConstants.RequiredFieldErrorMsg)]
            [Display(Name = "Парола")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Запомни ме?")]
            public bool RememberMe { get; set; }

            [Required]
            public string Token { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var recaptchaResponse = await this.reCaptchaService.ValidateResponse(this.Input.Token);

            if (recaptchaResponse.Success == false && recaptchaResponse.Score < 0.5)
            {
                this.ModelState.AddModelError(string.Empty, MessageConstants.FailedRecaptchaMsg);

                return this.Page();
            }

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Имейлът и паролата не съвпадат.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
