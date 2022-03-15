namespace ViewModels.Slot
{
    using System;

    using ViewModels.Address;

    public class SlotViewModel
    {
        public int Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Status { get; set; }

        public string AddressCityName { get; set; }

        public int AddressCityId { get; set; }
    }
}
