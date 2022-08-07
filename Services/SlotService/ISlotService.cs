namespace Services.SlotService;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using ViewModels.Slot;

public interface ISlotService
{
    Task<OperationResult> GenerateSlots(DateTime startDate, DateTime endDate, int slotDurationMinutes, int addressId);
    
    Task<OperationResult> EditSlot(int id, string status, string text);

    Task<int> DeleteSlotById(int id);

    Task<ICollection<SlotViewModel>> GetGabrovoSlots();

    Task<ICollection<SlotViewModel>> GetPlevenSlots();

    Task<ICollection<SlotViewModel>> GetFreePlevenSlots();

    Task<ICollection<SlotViewModel>> GetFreeGabrovoSlots();

    Task<ICollection<SlotViewModel>> GetTodaysTakenSlots();
}