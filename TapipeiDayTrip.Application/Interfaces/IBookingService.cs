using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDto> GetBookingByUserIdAsync(string id);
        Task<BookingDto> CreateBookingAsync(BookingDto bookingDto);
        // Task<bool> DeleteBookingAsync(int id);
    }
}