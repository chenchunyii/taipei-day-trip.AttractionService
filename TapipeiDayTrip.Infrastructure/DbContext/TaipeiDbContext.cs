using Microsoft.EntityFrameworkCore;
using taipei_day_trip_dotnet.Entity;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities;

namespace taipei_day_trip_dotnet.Data
{
    public class TaipeiDbContext : DbContext
    {
        public TaipeiDbContext(DbContextOptions<TaipeiDbContext> options) : base(options) { }

        public DbSet<Attraction> Attractions => Set<Attraction>();
        public DbSet<Booking> Bookings => Set<Booking>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Attraction entity
            modelBuilder.Entity<Attraction>(entity =>
            {
                entity.ToTable("webpage");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name).IsRequired();
                entity.Property(a => a.Category).IsRequired();
                entity.Property(a => a.Description).IsRequired();
                entity.Property(a => a.Address).IsRequired();
                entity.Property(a => a.Transport).IsRequired();
                entity.Property(a => a.Latitude).IsRequired();
                entity.Property(a => a.Longitude).IsRequired();
            });

            // Configure Booking entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.UserId).IsRequired();
                entity.Property(b => b.BookingDate).IsRequired();
                entity.Property(b => b.DayPeriod).IsRequired();
                entity.Property(b => b.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.HasOne(b => b.Attraction)
                      .WithMany(a => a.Bookings)
                      .HasForeignKey(b => b.AttractionId);
            });
        }
    }
}