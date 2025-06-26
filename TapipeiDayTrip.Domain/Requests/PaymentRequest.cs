using System.ComponentModel.DataAnnotations;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Commons;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Requests
{
    public class PaymentRequest
    {
        [Required]
        public string Prime { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public Cardholder Cardholder { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AccountEmail { get; set; }
    }
}