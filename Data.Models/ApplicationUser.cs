namespace Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        [ForeignKey(nameof(Patient))]
        public string PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedOn { get; set; }

        [ForeignKey(nameof(Image))]
        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; } = new HashSet<IdentityUserRole<string>>();

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new HashSet<IdentityUserClaim<string>>();

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; } = new HashSet<IdentityUserLogin<string>>();

        public virtual ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();

        public virtual ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>();

        public ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();

        public virtual ICollection<ChatImage> ChatImages { get; set; } = new HashSet<ChatImage>();
    }
}
