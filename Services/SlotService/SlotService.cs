namespace Services.SlotService
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

    using ViewModels.Slot;

    public class SlotService : ISlotService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;
        public SlotService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<ICollection<SlotViewModel>> GenerateSlots(DateTime start, DateTime end, int slotDurationMinutes)
        {
            var slots = new List<AppointmentSlot>();

            for (var slotStart = start; slotStart < end; slotStart = slotStart.AddMinutes(slotDurationMinutes))
            {
                var slotEnd = slotStart.AddMinutes(slotDurationMinutes);

                var slot = new AppointmentSlot()
                {
                    Start = slotStart,
                    End = slotEnd
                };

                slots.Add(slot);
            }

            await this.data.AppointmentSlots.AddRangeAsync(slots);
            await this.data.SaveChangesAsync();

            var slotsModel = this.mapper.Map<ICollection<SlotViewModel>>(slots);

            return slotsModel;
        }

        public async Task<ICollection<SlotViewModel>> GetSlots()
            => await this.data.AppointmentSlots
                        .Where(x => x.IsDeleted == false)
                        .ProjectTo<SlotViewModel>(this.mapper.ConfigurationProvider)
                        .ToListAsync();

        public async Task<int> DeleteSlotById(int id)
        {
            var slot = await this.data.AppointmentSlots
                                .FindAsync(id);

            slot.IsDeleted = true;
            await this.data.SaveChangesAsync();

            return slot.Id;
        }

        public async Task<bool> EditSlot(int id, string status)
        {
            var slot = await this.data.AppointmentSlots
                .FirstOrDefaultAsync(x => x.Id == id);

            if (slot == null)
            {
                return false;
            }

            slot.Status = status;

            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
