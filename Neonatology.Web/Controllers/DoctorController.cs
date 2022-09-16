﻿namespace Neonatology.Web.Controllers;

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.CityService;
using Services.DoctorService;

using ViewModels.Doctor;
using ViewModels.Slot;

using static Common.Constants.GlobalConstants;

[Authorize(Roles = DoctorConstants.DoctorRoleName)]
public class DoctorController : BaseController
{
    private readonly IDoctorService doctorService;
    private readonly ICityService cityService;
    private readonly IMapper mapper;

    public DoctorController(IDoctorService doctorService, ICityService cityService, IMapper mapper)
    {
        this.doctorService = doctorService;
        this.cityService = cityService;
        this.mapper = mapper;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Profile()
    {
        var doctorId = await this.doctorService.GetDoctorId();
        var viewModel = await this.doctorService.GetDoctorById(doctorId);

        return this.View(viewModel);
    }

    public async Task<IActionResult> Edit()
    {
        var doctorId = await this.doctorService.GetDoctorId();
        var doctorInfo = await this.doctorService.GetDoctorById(doctorId);

        var model = new DoctorEditFormModel()
        {
            Id = doctorInfo.Id,
            FirstName = doctorInfo.FullName.Split(' ')[0],
            LastName = doctorInfo.FullName.Split(' ')[1],
            PhoneNumber = doctorInfo.PhoneNumber,
            Age = doctorInfo.Age,
            Biography = doctorInfo.Biography,
            UserImageUrl = doctorInfo.UserImageUrl,
            Email = doctorInfo.Email,
            YearsOfExperience = doctorInfo.YearsOfExperience,
            Cities = await this.cityService.GetAllCities(),
            Addresses = await this.doctorService.GetDoctorAddressesById(doctorInfo.Id)
        };

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(DoctorEditFormModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var editModel = this.mapper.Map<DoctorEditModel>(model);
        var result = await this.doctorService.EditDoctorAsync(editModel);

        if (result.Failed)
        {
            this.TempData["Message"] = result.Error;
                
            return this.View(model);
        }

        return this.RedirectToAction(nameof(this.Profile));
    }

    public async Task<IActionResult> Calendar()
    {
        var doctorId = await this.doctorService.GetDoctorId();
        return this.View(new SlotEditModel()
        {
            Cities = await this.cityService.GetDoctorAddressesByDoctorId(doctorId)
        });
    }
}