
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Enum;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Reponses
{
    public class BookingWithAttractionResponse
    {
        public DateTime BookingDate { get; set; }
        public DayPeriodEnum DayPeriod { get; set; }
        public Decimal Amount { get; set; }
        public string AttractionName { get; set; }
        public string AttractionAddress { get; set; }
        public string AttractionImages { get; set; }
    }
}