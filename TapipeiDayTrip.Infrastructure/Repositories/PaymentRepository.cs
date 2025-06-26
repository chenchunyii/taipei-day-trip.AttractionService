using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities;
using taipei_day_trip_dotnet.Data;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {

        public async Task<bool> CreatePaymentWithTransactionAsync(
            PaymentDto paymentDto,
            BookingWithAttractionDto bookingWithAttractionDto,
            string orderNumber,
            IDbConnection connection,
            IDbTransaction transaction
        )
        {
            string sql = @"
        INSERT INTO Payments (
            AccountEmail,
            OrderNumber,
            OrderPrice,
            AttractionId,
            AttractionName,
            AttractionAddress,
            AttractionImage,
            TripDate,
            TripTime,
            ContactName,
            ContactEmail,
            ContactPhone,
            Status
        ) VALUES (
            @AccountEmail,
            @OrderNumber,
            @OrderPrice,
            @AttractionId,
            @AttractionName,
            @AttractionAddress,
            @AttractionImage,
            @TripDate,
            @TripTime,
            @ContactName,
            @ContactEmail,
            @ContactPhone,
            @Status
        )";

            int rowsAffected = await connection.ExecuteAsync(sql, new
            {
                paymentDto.AccountEmail,
                OrderNumber = orderNumber,
                OrderPrice = bookingWithAttractionDto.Amount,
                bookingWithAttractionDto.AttractionId,
                bookingWithAttractionDto.AttractionName,
                bookingWithAttractionDto.AttractionAddress,
                AttractionImage = bookingWithAttractionDto.AttractionImages,
                TripDate = bookingWithAttractionDto.BookingDate,
                TripTime = bookingWithAttractionDto.DayPeriod,
                ContactName = paymentDto.Cardholder.Name,
                ContactEmail = paymentDto.Cardholder.Email,
                ContactPhone = paymentDto.Cardholder.PhoneNumber,
                Status = 1
            }, transaction);

            return rowsAffected > 0;
        }
    }
}