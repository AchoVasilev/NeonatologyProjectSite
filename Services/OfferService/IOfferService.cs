namespace Services.OfferService;

using System.Collections.Generic;
using System.Threading.Tasks;

using ViewModels.Administration.Offer;
using ViewModels.Offer;

public interface IOfferService
{
    Task<ICollection<OfferViewModel>> GetAllAsync();

    Task<int> GetOnlineConsultationId();

    Task<OfferViewModel> GetOnlineConsultationModelAsync();

    Task<bool> DeleteOffer(int offerId);

    Task AddOffer(CreateOfferFormModel model);

    Task<EditOfferFormModel> GetOffer(int id);

    Task<bool> EditOffer(EditOfferFormModel model);
}