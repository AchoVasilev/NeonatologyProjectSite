namespace Services.OfferService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Administration.Offer;
    using ViewModels.Offer;

    public class OfferService : IOfferService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;
        private const string onlineConsultationName = "Онлайн консултация";

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
                        .Where(x => x.Name == onlineConsultationName && x.IsDeleted == false)
                        .Select(x => x.Id)
                        .FirstOrDefaultAsync();

        public async Task<OfferViewModel> GetOnlineConsultationModelAsync() 
            => await this.data.OfferedServices
                        .Where(x => x.Name == onlineConsultationName && x.IsDeleted == false)
                        .AsNoTracking()
                        .ProjectTo<OfferViewModel>(this.mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();

        public async Task<bool> DeleteOffer(int offerId)
        {
            var model = await this.data.OfferedServices
                .Where(x => x.Id == offerId && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return false;
            }

            model.IsDeleted = true;
            model.DeletedOn = DateTime.UtcNow;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task AddOffer(CreateOfferFormModel model)
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

        public async Task<bool> EditOffer(EditOfferFormModel model)
        {
            var offer = await this.data.OfferedServices
                                   .Where(x => x.Id == model.Id && x.IsDeleted == false)
                                   .FirstOrDefaultAsync();

            if (offer == null)
            {
                return false;
            }

            offer.Name = model.Name;
            offer.Price = model.Price;
            offer.ModifiedOn = DateTime.UtcNow;

            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
