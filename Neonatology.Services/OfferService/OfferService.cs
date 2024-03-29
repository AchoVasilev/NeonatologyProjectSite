﻿namespace Neonatology.Services.OfferService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Neonatology.Common.Models;
using ViewModels.Administration.Offer;
using ViewModels.Offer;
using static Neonatology.Common.Constants.GlobalConstants.MessageConstants;

public class OfferService : IOfferService
{
    private readonly NeonatologyDbContext data;
    private readonly IMapper mapper;
    private const string OnlineConsultationName = "Онлайн консултация";

    public OfferService(NeonatologyDbContext data, IMapper mapper)
    {
        this.data = data;
        this.mapper = mapper;
    }

    public async Task<ICollection<OfferViewModel>> GetAllAsync()
        => await this.data.OfferedServices
            .Where(x => x.IsDeleted == false)
            .OrderByDescending(x => x.CreatedOn)
            .AsNoTracking()
            .ProjectTo<OfferViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<int> GetOnlineConsultationId()
        => await this.data.OfferedServices
            .Where(x => x.Name == OnlineConsultationName && x.IsDeleted == false)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

    public async Task<OfferViewModel> GetOnlineConsultationModelAsync() 
        => await this.data.OfferedServices
            .Where(x => x.Name == OnlineConsultationName && x.IsDeleted == false)
            .AsNoTracking()
            .ProjectTo<OfferViewModel>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<OperationResult> DeleteOffer(int offerId)
    {
        var model = await this.data.OfferedServices
            .Where(x => x.Id == offerId && x.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (model is null)
        {
            return OfferDoesNotExistErrorMsg;
        }

        model.IsDeleted = true;
        model.DeletedOn = DateTime.UtcNow;

        await this.data.SaveChangesAsync();

        return true;
    }

    public async Task AddOffer(CreateOfferServiceModel model)
    {
        var offer = new OfferedService
        {
            Name = model.Name,
            Price = model.Price
        };

        await this.data.OfferedServices.AddAsync(offer);
        await this.data.SaveChangesAsync();
    }

    public async Task<EditOfferFormModel> GetOffer(int id)
        => await this.data.OfferedServices
            .Where(x => x.Id == id && x.IsDeleted == false)
            .AsNoTracking()
            .ProjectTo<EditOfferFormModel>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<OperationResult> EditOffer(EditOfferServiceModel model)
    {
        var offer = await this.data.OfferedServices
            .Where(x => x.Id == model.Id && x.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (offer is null)
        {
            return OfferDoesNotExistErrorMsg;
        }

        offer.Name = model.Name;
        offer.Price = model.Price;
        offer.ModifiedOn = DateTime.UtcNow;

        await this.data.SaveChangesAsync();

        return true;
    }
}