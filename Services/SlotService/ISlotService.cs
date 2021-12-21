namespace Services.SlotService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Slot;

    public interface ISlotService
    {
        Task<ICollection<SlotViewModel>> GenerateSlots(DateTime start, DateTime end, int slotDurationMinutes);

        Task<ICollection<SlotViewModel>> GetSlots();

        Task<bool> EditSlot(int id, string status);

        Task<int> DeleteSlotById(int id);
    }
}
