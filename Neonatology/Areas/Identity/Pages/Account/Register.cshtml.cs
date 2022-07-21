namespace Neonatology.Areas.Identity.Pages.Account
{
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using static Common.GlobalConstants.MessageConstants;
    using static Common.GlobalConstants.AccountConstants;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly ReCaptchaService reCaptchaService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ReCaptchaService reCaptchaService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.reCaptchaService = reCaptchaService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = RequiredFieldErrorMsg)]
            [EmailAddress]
            [Display(Name = EmailName)]
            public string Email { get; set; }

            [Required(ErrorMessage = RequiredFieldErrorMsg)]
            [StringLength(100, ErrorMessage = PasswordLengthErrorMsg, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = PasswordName)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = RepeatPasswordName)]
            [Compare("Password", ErrorMessage = PasswordsNotMatchErrorMsg)]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Token { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var recaptchaResponse = await this.reCaptchaService.ValidateResponse(this.Input.Token);

            if (recaptchaResponse.Success == false && recaptchaResponse.Score < 0.5)
            {
                this.ModelState.AddModelError(string.Empty, FailedRecaptchaMsg);

                return this.Page();
            }

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = this.Input.Email,
                    Email = this.Input.Email,
                    EmailConfirmed = true
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    await this.userManager.AddToRoleAsync(user, Common.GlobalConstants.PatientRoleName);

                    await this.signInManager.SignInAsync(user, isPersistent: false);

                    return this.RedirectToAction("Finish", "Patient", new { area = "" });
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
