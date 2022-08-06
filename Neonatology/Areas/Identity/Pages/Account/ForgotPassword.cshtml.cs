// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace Neonatology.Areas.Identity.Pages.Account;

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using static Common.GlobalConstants.AccountConstants;
using static Common.GlobalConstants.MessageConstants;

public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IEmailSender emailSender;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        this.userManager = userManager;
        this.emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [EmailAddress]
        [Display(Name = EmailName)]
        public string Email { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (this.ModelState.IsValid)
        {
            var user = await this.userManager.FindByEmailAsync(this.Input.Email);
            if (user == null || !(await this.userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return this.RedirectToPage("./ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = this.Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: this.Request.Scheme);

            await this.emailSender.SendEmailAsync(this.Input.Email,
                "Рестартиране на парола",
                $"Променете паролата си като <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>цъкнете тук</a>.");

            return this.RedirectToPage("./ForgotPasswordConfirmation");
        }

        return this.Page();
    }
}