using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Responses
{
    public class PaymentResponse
    {
        public string OrderNumber { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}