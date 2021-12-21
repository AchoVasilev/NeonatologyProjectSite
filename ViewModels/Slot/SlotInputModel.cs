namespace ViewModels.Slot
{
    using System;

    public class SlotInputModel
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int SlotDurationMinutes { get; set; }
    }
}
