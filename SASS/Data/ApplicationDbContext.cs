using Microsoft.EntityFrameworkCore;
using SASS.Models;

namespace SASS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<AppointmentLogs> AppointmentLogs { get; set; }
        public DbSet<Reminders> Reminders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure EF Core recognizes ENUM columns
            modelBuilder.Entity<Users>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Appointments>()
                .Property(a => a.Status)
                .HasConversion<string>();
        }
    }
}
