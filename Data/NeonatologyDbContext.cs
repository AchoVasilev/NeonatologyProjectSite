namespace Data
{
    using System.Linq;

    using Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class NeonatologyDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public NeonatologyDbContext(DbContextOptions<NeonatologyDbContext> options)
        : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<AppointmentSlot> AppointmentSlots { get; set; }

        public DbSet<OfferedService> OfferedServices { get; set; }

        public DbSet<AppointmentCause> AppointmentCauses { get; set; }

        public DbSet<StripeCheckoutSession> StripeCheckoutSessions { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationType> NotificationTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(r => r.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId);

            builder
                .Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(s => s.SentMessages)
                .HasForeignKey(m => m.SenderId);

            var entityTypes = builder.Model.GetEntityTypes().ToList();
            var foreignKeys = entityTypes
                    .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }
    }
}
