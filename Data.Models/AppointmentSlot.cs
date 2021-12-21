namespace Data.Models
{
    using System;

    using Data.Common.Models;

    public class AppointmentSlot : BaseModel<int>
    {
        public int Id { get; init; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Status { get; set; } = "Свободен";

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
