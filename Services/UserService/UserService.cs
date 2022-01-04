﻿namespace Services.UserService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Chat;

    public class UserService : IUserService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public UserService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ChatUserViewModel>> GetAllChatUsers()
            => await this.data.ApplicationUsers
                    .ProjectTo<ChatUserViewModel>(this.mapper.ConfigurationProvider)
                    .ToListAsync();

        public async Task<ChatUserViewModel> GetChatUserById(string id)
            => await this.data.ApplicationUsers
                        .Where(x => x.Id == id && x.IsDeleted == false)
                        .ProjectTo<ChatUserViewModel>(this.mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();

        public async Task<string> GetUserIdByDoctorIdAsync(string doctorId)
            => await this.data.ApplicationUsers
                        .Where(x => x.DoctorId == doctorId && x.IsDeleted == false)
                        .Select(x => x.Id)
                        .FirstOrDefaultAsync();
    }
}