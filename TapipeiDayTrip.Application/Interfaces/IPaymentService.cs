using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Responses;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentAsync(PaymentDto paymentDto);

    }
}