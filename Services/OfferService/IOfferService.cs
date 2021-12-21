namespace Services.OfferService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Offer;

    public interface IOfferService
    {
        Task<ICollection<OfferViewModel>> GetAllAsync();
    }
}
