namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    public class Image : BaseModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string RemoteImageUrl { get; set; }

        public string Url { get; set; }

        public string Extension { get; set; }

        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
