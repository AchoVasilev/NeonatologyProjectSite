namespace Services.FeedbackService
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

    using ViewModels.Feedback;

    public class FeedbackService : IFeedbackService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public FeedbackService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task CreateFeedback(FeedbackInputModel model)
        {
            var feedback = new Feedback
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Type = model.Type,
                Comment = model.Comment
            };

            await this.data.Feedbacks.AddAsync(feedback);
            await this.data.SaveChangesAsync();
        }

        public async Task<ICollection<FeedbackViewModel>> GetUserFeedbacks(string email) 
            => await this.data.Feedbacks
                .Where(x => x.Email == email && x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<FeedbackViewModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ICollection<FeedbackViewModel>> GetAll()
             => await this.data.Feedbacks
                    .Where(x => x.IsDeleted == false)
                    .OrderByDescending(x => x.CreatedOn)
                    .ProjectTo<FeedbackViewModel>(this.mapper.ConfigurationProvider)
                    .ToListAsync();

        public async Task<bool> Delete(int id)
        {
            var model = await this.data.Feedbacks
                    .Where(x => x.Id == id && x.IsDeleted == false)
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

        public async Task<FeedbackViewModel> GetById(int id)
            => await this.data.Feedbacks
                .Where(x => x.Id == id && x.IsDeleted == false)
                .ProjectTo<FeedbackViewModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task SolveFeedback(int id)
        {
            var feedback = await this.data.Feedbacks
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

            feedback.IsSolved = true;
            feedback.ModifiedOn = DateTime.UtcNow;

            await this.data.SaveChangesAsync();
        }
    }
}
