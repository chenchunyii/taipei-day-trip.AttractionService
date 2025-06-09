using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Infrastructure.Repositories
{
    public interface IBookingRepository
    {
        Task<BookingWithAttractionDto> CreateBookingWithAttractionAsync(BookingDto bookingDto);
        Task<BookingWithAttractionDto> GetBookingByUserIdAsync(string id);
        // Task<bool> DeleteBookingAsync(int id);

    }
}