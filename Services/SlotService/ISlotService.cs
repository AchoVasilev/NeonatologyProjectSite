namespace Services.SlotService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Slot;

    public interface ISlotService
    {
        Task<ICollection<SlotViewModel>> GenerateSlots(DateTime start, DateTime end, int slotDurationMinutes, int addressId);

        Task<ICollection<SlotViewModel>> GetGabrovoSlots();

        Task<ICollection<SlotViewModel>> GetPlevenSlots();

        Task<ICollection<SlotViewModel>> GetPatientSlots();

        Task<bool> EditSlot(int id, string status);

        Task<int> DeleteSlotById(int id);
    }
}
