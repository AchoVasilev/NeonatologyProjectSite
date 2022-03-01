namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class AppointmentSlot : BaseModel<int>
    {
        [MaxLength(DefaultMaxLength)]
        public string Text { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Status { get; set; } = "Свободен";
    }
}
