namespace Neonatology.ViewModels.Slot;

using System;
using Common.Mapping;
using Data.Models;

public class SlotViewModel : IMapFrom<AppointmentSlot>
{
    public int Id { get; init; }

    public DateTime Start { get; init; }

    public DateTime End { get; init; }

    public string Status { get; init; }

    public string Text { get; init; }

    public int AddressId { get; init; }

    public string AddressCityName { get; init; }

    public int AddressCityId { get; init; }
}