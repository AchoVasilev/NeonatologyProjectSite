namespace Services.SlotService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Slot;

    public interface ISlotService
    {
        Task<bool> GenerateSlots(DateTime start, DateTime end, int slotDurationMinutes, int addressId);

        Task<ICollection<SlotViewModel>> GetGabrovoSlots();

        Task<ICollection<SlotViewModel>> GetPlevenSlots();

        Task<ICollection<SlotViewModel>> GetFreePlevenSlots();

        Task<ICollection<SlotViewModel>> GetFreeGabrovoSlots();

        Task<ICollection<SlotViewModel>> GetTodaysTakenSlots();

        Task<bool> EditSlot(int id, string status, string text);

        Task<int> DeleteSlotById(int id);
    }
}
