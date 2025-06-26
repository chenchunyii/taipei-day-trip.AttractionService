using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Reponses;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingWithAttractionResponse> GetBookingByUserIdAsync(string userId);
        Task<BookingWithAttractionResponse> CreateBookingAsync(BookingDto bookingDto);
        Task<bool> DeleteBookingByUserIdAsync(string userId);
    }
}