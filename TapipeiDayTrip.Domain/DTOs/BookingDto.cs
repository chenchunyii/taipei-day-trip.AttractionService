using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Enum;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public long AttractionId { get; set; }
        public DateTime BookingDate { get; set; }
        public int DayPeriod { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}