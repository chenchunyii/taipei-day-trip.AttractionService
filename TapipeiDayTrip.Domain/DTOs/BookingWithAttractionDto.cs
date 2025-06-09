using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Enum;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs
{
    public class BookingWithAttractionDto
    {
        public DateTime BookingDate { get; set; }
        public int DayPeriod { get; set; }
        public Decimal Amount { get; set; }
        public string AttractionName { get; set; }
        public string AttractionAddress { get; set; }
        public string AttractionImages { get; set; }
    }
}