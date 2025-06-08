using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using taipei_day_trip_dotnet.Data;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TaipeiDbContext _dbContext;
        private readonly string _connectionString;
        public BookingRepository(TaipeiDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Booking> CreateBookingAsync(BookingDto bookingDto)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Insert booking record and get the generated ID
                string sql = @"INSERT INTO booking (UserId, AttractionId, BookingDate, DayPeriod, Amount, CreatedAt, UpdatedAt)
                      VALUES (@UserId, @AttractionId, @Date, @Time, @Price, @ContactName, @ContactEmail, @ContactPhone);
                      SELECT LAST_INSERT_ID();";

                // Execute the query and get the generated booking ID
                int bookingId = await connection.ExecuteScalarAsync<int>(sql, new
                {
                    bookingDto.UserId,
                    bookingDto.AttractionId,
                    bookingDto.BookingDate,
                    bookingDto.DayPeriod,
                    bookingDto.Amount,
                    bookingDto.CreatedAt,
                    bookingDto.UpdatedAt,
                });

                // Create and return the newly created Booking entity
                return new Booking
                {
                    Id = bookingId,
                    UserId = bookingDto.UserId,
                    AttractionId = bookingDto.AttractionId,
                    BookingDate = bookingDto.BookingDate,
                    DayPeriod = bookingDto.DayPeriod,
                    Amount = bookingDto.Amount,
                    CreatedAt = bookingDto.CreatedAt,
                    UpdatedAt = bookingDto.UpdatedAt,
                };
            }
        }

        public async Task<Booking> GetBookingByUserIdAsync(string id)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM booking WHERE user_id = @id";
                var result = (await connection.QueryAsync<Booking>(sql, new { id })).FirstOrDefault();
                if (result == null)
                {
                    return null;
                }

                return result;
            }
        }

    }
}