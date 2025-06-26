using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Infrastructure.Repositories
{
    public interface IBookingRepository
    {
        Task<BookingWithAttractionDto> CreateBookingWithAttractionAsync(BookingDto bookingDto);
        Task<BookingWithAttractionDto> GetBookingByUserIdAsync(string userId);
        Task<bool> DeleteBookingByUserIdAsync(string userId);
        // For IBookingRepository.cs
        Task<BookingWithAttractionDto> UpdateBookingStatusWithTransactionAsync(string userId, IDbConnection connection, IDbTransaction transaction);
    }
}