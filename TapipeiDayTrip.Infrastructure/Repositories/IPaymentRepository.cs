using System.Data;
using System.Threading.Tasks;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Infrastructure.Repositories
{
    public interface IPaymentRepository
    {
        // For IPaymentRepository.cs
        Task<bool> CreatePaymentWithTransactionAsync(PaymentDto paymentDto, BookingWithAttractionDto bookingWithAttractionDto, string orderNumber, IDbConnection connection, IDbTransaction transaction);
    }
}