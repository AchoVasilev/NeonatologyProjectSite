namespace Data.Common.Models
{
    using System;

    public interface BaseModel<TKey>
    {
        public TKey Id { get; init; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
