namespace Services.MessageService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
using Common;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Chat;

    public class MessageService : IMessageService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public MessageService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task CreateMessageAsync(string content, string senderId, string receiverId)
        {
            var message = new Message
            {
                Content = content,
                SenderId = senderId,
                ReceiverId = receiverId,
            };

            await this.data.Messages.AddAsync(message);
            await this.data.SaveChangesAsync();
        }

        public async Task<ICollection<ChatConversationsViewModel>> GetAllMessages(string userId)
        {
            var sentMessages = this.data.Messages
                .Where(x => x.IsDeleted == false && (x.SenderId == userId || x.ReceiverId == userId))
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => x.Sender)
                .AsQueryable();

            var receivedMessages = this.data.Messages
                .Where(x => x.IsDeleted == false && (x.SenderId == userId || x.ReceiverId == userId))
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => x.Receiver)
                .AsQueryable();

            var concatMessages = await sentMessages
                .Concat(receivedMessages)
                .Where(x => x.Id != userId)
                .Distinct()
                .ProjectTo<ChatConversationsViewModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return concatMessages;
        }

        public async Task<string> GetLastMessage(string currentUserId, string userId)
            => await this.data.Messages
                        .Where(m => m.IsDeleted == false &&
                        ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                        m.ReceiverId == userId && m.SenderId == currentUserId))
                        .OrderByDescending(x => x.CreatedOn)
                        .Select(x => x.Content)
                        .FirstOrDefaultAsync();

        public async Task<string> GetLastActivityAsync(string currentUserId, string userId)
            => await this.data.Messages
                            .Where(m => !m.IsDeleted &&
                                        ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                                         (m.ReceiverId == userId && m.SenderId == currentUserId)))
                            .OrderByDescending(m => m.CreatedOn)
                            .Select(m => m.CreatedOn.AddHours(3).ToString(GlobalConstants.DateTimeFormats.DateTimeFormat))
                            .FirstOrDefaultAsync();

        public async Task<IEnumerable<ChatMessageWithUserViewModel>> GetAllWithUserAsync(string currentUserId, string userId)
            => await this.data.Messages
                .Where(m => !m.IsDeleted &&
                            ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                             (m.ReceiverId == userId && m.SenderId == currentUserId)))
                .OrderBy(m => m.CreatedOn)
                .ProjectTo<ChatMessageWithUserViewModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
