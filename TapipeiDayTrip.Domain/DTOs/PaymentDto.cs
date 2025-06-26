using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Commons;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs
{
    public class PaymentDto
    {
        public string Prime { get; set; }
        public int Amount { get; set; }
        public Cardholder Cardholder { get; set; }
        public string UserId { get; set; }
        public string AccountEmail { get; set; }
    }
}