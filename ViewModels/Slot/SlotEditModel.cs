namespace ViewModels.Slot;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Address;

using static Data.Common.DataConstants.Constants;

public class SlotEditModel
{
    public int Id { get; set; }

    [MaxLength(DefaultMaxLength)]
    public string Text { get; set; }

    public string Status { get; set; }

    public int AddressId { get; set; }

    public ICollection<AddressFormModel> Cities { get; set; }
}