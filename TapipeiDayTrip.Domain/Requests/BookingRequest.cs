using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Enum;
namespace taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Requests
{
    public class BookingRequest
    {
        public string UserId { get; set; }
        public long AttractionId { get; set; }
        public DateTime BookingDate { get; set; }
        public DayPeriodEnum DayPeriod { get; set; }
        public decimal Amount { get; set; }
    }
}