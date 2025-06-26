using taipei_day_trip_dotnet.Entity;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Enum;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public long AttractionId { get; set; }
        public DateTime BookingDate { get; set; }
        public int DayPeriod { get; set; }
        public decimal Amount { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        public Attraction Attraction { get; set; } = null!;
    }
}