namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class AppointmentSlot : BaseModel<int>
    {
        public int Id { get; init; }

        [MaxLength(DefaultMaxLength)]
        public string Text { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Status { get; set; } = "Свободен";

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
