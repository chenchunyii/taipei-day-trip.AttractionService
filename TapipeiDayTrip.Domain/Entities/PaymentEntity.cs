using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using taipei_day_trip_dotnet.Entity;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities
{
    [Table("Payments")]
    public class PaymentEntity
    {
        [Required]
        [MaxLength(255)]
        public string AccountEmail { get; set; }

        [Key]
        [Required]
        [MaxLength(100)]
        public string OrderNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal OrderPrice { get; set; }

        [Required]
        public long AttractionId { get; set; }

        [Required]
        [MaxLength(255)]
        public string AttractionName { get; set; }

        [Required]
        public string AttractionAddress { get; set; }

        [Required]
        public string AttractionImage { get; set; }

        [Required]
        public DateTime TripDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string TripTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContactName { get; set; }

        [Required]
        [MaxLength(255)]
        public string ContactEmail { get; set; }

        [Required]
        [MaxLength(20)]
        public string ContactPhone { get; set; }

        public int Status { get; set; }

        [ForeignKey("AttractionId")]
        public Attraction Attraction { get; set; }
    }
}