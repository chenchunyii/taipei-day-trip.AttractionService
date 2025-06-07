using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities;

namespace taipei_day_trip_dotnet.Entity
{
    [Table("webpage")]
    public class Attraction
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("NAME")]
        public string Name { get; set; } = null!;

        [Required]
        [Column("CATEGORY")]
        public string Category { get; set; } = null!;

        [Required]
        [Column("DESCRIPTION")]
        public string Description { get; set; } = null!;

        [Required]
        [Column("ADDRESS")]
        public string Address { get; set; } = null!;

        [Required]
        [Column("TRANSPORT")]
        public string Transport { get; set; } = null!;

        [Column("MRT")]
        public string? MRT { get; set; }

        [Required]
        [Column("LAT")]
        public double Latitude { get; set; }

        [Required]
        [Column("LNG")]
        public double Longitude { get; set; }

        [Column("IMAGES")]
        public string? Images { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}