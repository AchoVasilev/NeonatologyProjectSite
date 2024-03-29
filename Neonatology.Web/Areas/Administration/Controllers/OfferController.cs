﻿namespace Neonatology.Web.Areas.Administration.Controllers;

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.OfferService;
using ViewModels.Administration.Offer;
using static Common.Constants.GlobalConstants.MessageConstants;

public class OfferController : BaseController
{
    private readonly IOfferService offerService;
    private readonly IMapper mapper;
    public OfferController(IOfferService offerService, IMapper mapper)
    {
        this.offerService = offerService;
        this.mapper = mapper;
    }

    public async Task<IActionResult> All()
    {
        var services = await this.offerService.GetAllAsync();

        return this.View(services);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await this.offerService.DeleteOffer(id);
        if (isDeleted.Failed)
        {
            this.TempData["Message"] = isDeleted.Error;
        }
        else
        {
            this.TempData["Message"] = SuccessfulDeleteMsg;
        }

        return this.RedirectToAction(nameof(this.All));
    }

    public IActionResult Add()
        => this.View(new CreateOfferFormModel());

    [HttpPost]
    public async Task<IActionResult> Add(CreateOfferFormModel model)
    {
        var serviceModel = this.mapper.Map<CreateOfferServiceModel>(model);
        await this.offerService.AddOffer(serviceModel);

        this.TempData["Message"] = SuccessfulAddedItemMsg;

        return this.RedirectToAction(nameof(this.All));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await this.offerService.GetOffer(id);

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditOfferFormModel model)
    {
        var serviceModel = this.mapper.Map<EditOfferServiceModel>(model);
        var isEdited = await this.offerService.EditOffer(serviceModel);
        
        if (isEdited.Failed)
        {
            this.TempData["Message"] = isEdited.Error;
        }
        else
        {
            this.TempData["Message"] = SuccessfulEditMsg;
        }

        return this.RedirectToAction(nameof(this.All));
    }
}