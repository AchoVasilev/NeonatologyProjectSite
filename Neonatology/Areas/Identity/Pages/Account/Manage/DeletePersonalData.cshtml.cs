// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Neonatology.Areas.Identity.Pages.Account.Manage;

public class DeletePersonalDataModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<DeletePersonalDataModel> _logger;
    private readonly NeonatologyDbContext data;

    public DeletePersonalDataModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<DeletePersonalDataModel> logger, 
        NeonatologyDbContext data)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._logger = logger;
        this.data = data;
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
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool RequirePassword { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Не успяхме да заредим потребител с номер '{this._userManager.GetUserId(this.User)}'.");
        }

        this.RequirePassword = await this._userManager.HasPasswordAsync(user);
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await this._userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Не успяхме да заредим потребител с номер '{this._userManager.GetUserId(this.User)}'.");
        }

        this.RequirePassword = await this._userManager.HasPasswordAsync(user);
        if (this.RequirePassword)
        {
            if (!await this._userManager.CheckPasswordAsync(user, this.Input.Password))
            {
                this.ModelState.AddModelError(string.Empty, "Грешна парола");
                return this.Page();
            }
        }

        var result = await this._userManager.DeleteAsync(user);
        var userId = await this._userManager.GetUserIdAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Стана грешка");
        }

        var getPatient = await this.data.Patients
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        getPatient.IsDeleted = true;
        await this.data.SaveChangesAsync();

        await this._signInManager.SignOutAsync();

        this._logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

        return this.Redirect("~/");
    }
}