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

        public async Task<BookingWithAttractionDto> CreateBookingWithAttractionAsync(BookingDto bookingDto)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert booking record and get the generated ID
                        string sql = @"INSERT INTO Bookings (UserId, AttractionId, BookingDate, DayPeriod, Amount, CreatedAt, UpdatedAt)
                          VALUES (@UserId, @AttractionId, @BookingDate, @DayPeriod, @Amount, @CreatedAt, @UpdatedAt);
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
                        }, transaction);

                        // Query to get booking with attraction details
                        string joinQuery = @"
                                SELECT b.*, 
                                    w.id AS Attraction_Id, 
                                    w.name AS Attraction_Name, 
                                    w.address AS Attraction_Address, 
                                    w.mrt AS Attraction_Mrt,
                                    w.images AS Attraction_Image
                                FROM Bookings b
                                JOIN webpage w ON b.AttractionId = w.id
                                WHERE b.Id = @BookingId";

                        var result = await connection.QueryAsync<dynamic>(joinQuery, new { BookingId = bookingId }, transaction);

                        transaction.Commit();

                        var booking = result.FirstOrDefault();
                        if (booking == null)
                        {
                            return null;
                        }

                        // Create and return the newly created BookingWithAttractionDto
                        return new BookingWithAttractionDto
                        {
                            BookingDate = booking.BookingDate,
                            DayPeriod = booking.DayPeriod,
                            Amount = booking.Amount,
                            AttractionName = booking.Attraction_Name,
                            AttractionAddress = booking.Attraction_Address,
                            AttractionImages = booking.Attraction_Image
                        };
                    }
                    catch (Exception)
                    {
                        // 檢查交易狀態，只有在未提交且未回滾時才回滾
                        if (transaction?.Connection != null)
                        {
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (InvalidOperationException)
                            {
                                // 交易已經被處理，忽略這個錯誤
                            }
                        }
                        throw;
                    }
                }
            }
        }

        public async Task<BookingWithAttractionDto> GetBookingByUserIdAsync(string id)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = @"
                    SELECT b.*, 
                        w.id AS AttractionId, 
                        w.name AS AttractionName, 
                        w.address AS AttractionAddress, 
                        w.mrt AS AttractionMrt,
                        w.images AS AttractionImages
                    FROM Bookings b
                    JOIN webpage w ON b.AttractionId = w.id
                    WHERE b.UserId = @Id
                    AND b.Status = 2
                    ORDER BY b.Id DESC
                    LIMIT 1";
                var booking = await connection.QueryFirstOrDefaultAsync<BookingWithAttractionDto>(sql, new { Id = id });

                if (booking == null)
                {
                    return null;
                }

                var result = new BookingWithAttractionDto
                {
                    BookingDate = booking.BookingDate,
                    DayPeriod = booking.DayPeriod,
                    Amount = booking.Amount,
                    AttractionName = booking.AttractionName,
                    AttractionAddress = booking.AttractionAddress,
                    AttractionImages = booking.AttractionImages,
                    AttractionId = booking.AttractionId
                };

                return result;
            }
        }

        public async Task<bool> DeleteBookingByUserIdAsync(string userId)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "UPDATE Bookings SET Status = 0 WHERE UserId = @UserId AND Status = 2";
                var rowsAffected = await connection.ExecuteAsync(sql, new { UserId = userId });
                return rowsAffected > 0;
            }
        }

        public async Task<BookingWithAttractionDto> UpdateBookingStatusWithTransactionAsync(
            string userId,
            IDbConnection connection,
            IDbTransaction transaction
        )
        {
            // Update booking status
            string updateSql = "UPDATE Bookings SET Status = 1 WHERE UserId = @UserId AND Status = 2";
            var rowsAffected = await connection.ExecuteAsync(updateSql, new { UserId = userId }, transaction);

            if (rowsAffected > 0)
            {
                // Then fetch the updated booking with attraction details
                string selectSql = @"
            SELECT b.*, 
                w.id AS AttractionId, 
                w.name AS AttractionName, 
                w.address AS AttractionAddress, 
                w.mrt AS AttractionMrt,
                w.images AS AttractionImages
            FROM Bookings b
            JOIN webpage w ON b.AttractionId = w.id
            WHERE b.UserId = @UserId
            AND b.Status = 1
            ORDER BY b.UpdatedAt DESC
            LIMIT 1";

                var booking = await connection.QueryFirstOrDefaultAsync<BookingWithAttractionDto>(selectSql,
                    new { UserId = userId }, transaction);

                if (booking != null)
                {
                    return new BookingWithAttractionDto
                    {
                        BookingDate = booking.BookingDate,
                        DayPeriod = booking.DayPeriod,
                        Amount = booking.Amount,
                        AttractionName = booking.AttractionName,
                        AttractionAddress = booking.AttractionAddress,
                        AttractionImages = booking.AttractionImages,
                        AttractionId = booking.AttractionId
                    };
                }
            }

            return null;
        }

    }
}