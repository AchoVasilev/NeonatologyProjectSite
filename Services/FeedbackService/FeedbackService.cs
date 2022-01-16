namespace Services.FeedbackService
{
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
                Type = model.Type
            };

            await this.data.Feedbacks.AddAsync(feedback);
            await this.data.SaveChangesAsync();
        }

        public async Task<ICollection<FeedbackViewModel>> GetUserFeedbacks(string email) 
            => await this.data.Feedbacks
                .Where(x => x.Email == email && x.IsDeleted == false)
                .ProjectTo<FeedbackViewModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
