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
    using Data.Models.Enums;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Slot;
    using static Common.GlobalConstants.DoctorConstants;

    public class SlotService : ISlotService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;
        public SlotService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<bool> GenerateSlots(DateTime start, DateTime end, int slotDurationMinutes, int addressId)
        {
            var slots = new List<AppointmentSlot>();

            for (var slotStart = start; slotStart < end; slotStart = slotStart.AddMinutes(slotDurationMinutes))
            {
                var slotEnd = slotStart.AddMinutes(slotDurationMinutes);

                var slot = new AppointmentSlot()
                {
                    Start = slotStart,
                    End = slotEnd,
                    AddressId = addressId,
                };

                if (await this.SlotExists(slot.Start, slot.End, slot.AddressId))
                {
                    continue;
                }

                slots.Add(slot);
            }

            if (slots.Count == 0)
            {
                return false;
            }

            await this.data.AppointmentSlots.AddRangeAsync(slots);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<SlotViewModel>> GetGabrovoSlots()
            => await this.data.AppointmentSlots
                        .Where(x => x.IsDeleted == false && x.Address.City.Name == GabrovoCityName)
                        .ProjectTo<SlotViewModel>(this.mapper.ConfigurationProvider)
                        .ToListAsync();

        public async Task<ICollection<SlotViewModel>> GetPlevenSlots()
            => await this.data.AppointmentSlots
                        .Where(x => x.IsDeleted == false && x.Address.City.Name == PlevenCityName)
                        .ProjectTo<SlotViewModel>(this.mapper.ConfigurationProvider)
                        .ToListAsync();

        public async Task<ICollection<SlotViewModel>> GetFreePlevenSlots()
            => await this.data.AppointmentSlots
            .Where(x => x.IsDeleted == false &&
                    x.Address.City.Name == PlevenCityName &&
                    x.Start.Date >= DateTime.UtcNow.Date &&
                    x.End <= DateTime.UtcNow.AddDays(20).Date &&
                    x.Status == SlotStatus.Свободен.ToString())
            .ProjectTo<SlotViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<ICollection<SlotViewModel>> GetFreeGabrovoSlots()
            => await this.data.AppointmentSlots
            .Where(x => x.IsDeleted == false &&
                    x.Address.City.Name == GabrovoCityName &&
                    x.Start.Date >= DateTime.UtcNow.Date &&
                    x.End.Date <= DateTime.UtcNow.AddDays(20).Date &&
                    x.Status == SlotStatus.Свободен.ToString())
            .ProjectTo<SlotViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<int> DeleteSlotById(int id)
        {
            var slot = await this.data.AppointmentSlots
                                .FirstOrDefaultAsync(x => x.Id == id);

            if (slot is null)
            {
                return 0;
            }

            slot.IsDeleted = true;
            slot.DeletedOn = DateTime.UtcNow;

            await this.data.SaveChangesAsync();

            return slot.Id;
        }

        public async Task<bool> EditSlot(int id, string status, string text)
        {
            var slot = await this.data.AppointmentSlots
                .FirstOrDefaultAsync(x => x.Id == id);

            if (slot == null)
            {
                return false;
            }

            slot.Status = status;
            slot.Text = text;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<SlotViewModel>> GetTodaysTakenSlots() 
            => await this.data.AppointmentSlots
                .Where(x => x.IsDeleted == false && 
                    x.Start.Date == DateTime.UtcNow.Date && 
                    x.Status == SlotStatus.Зает.ToString())
                .ProjectTo<SlotViewModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        private async Task<bool> SlotExists(DateTime start, DateTime end, int addressId)
        {
            var result = await this.data.AppointmentSlots
                .FirstOrDefaultAsync(x => x.Start == start && 
                                    x.End == end &&
                                    x.AddressId == addressId &&
                                    x.IsDeleted == false);

            if (result != null)
            {
                return true;
            }

            return false;
        }
    }
}
