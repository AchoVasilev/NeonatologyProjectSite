﻿namespace Neonatology.Services.OfferService;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Neonatology.Common.Models;
using ViewModels.Administration.Offer;
using ViewModels.Offer;

public interface IOfferService : ITransientService
{
    Task<ICollection<OfferViewModel>> GetAllAsync();

    Task<int> GetOnlineConsultationId();

    Task<OfferViewModel> GetOnlineConsultationModelAsync();

    Task<OperationResult> DeleteOffer(int offerId);

    Task AddOffer(CreateOfferServiceModel model);

    Task<EditOfferFormModel> GetOffer(int id);

    Task<OperationResult> EditOffer(EditOfferServiceModel model);
}