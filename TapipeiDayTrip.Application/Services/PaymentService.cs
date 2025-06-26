using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MySqlConnector;
using taipei_day_trip_dotnet.Data;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application.Interfaces;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Responses;
using taipei_day_trip_dotnet.TapipeiDayTrip.Infrastructure.Repositories;


namespace taipei_day_trip_dotnet.TapipeiDayTrip.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PaymentService> _logger;
        private readonly TaipeiDbContext _dbContext;
        private const string TapPayApiUrl = "https://sandbox.tappaysdk.com/tpc/payment/pay-by-prime";
        private const string PartnerKey = "partner_IFyYKSZbeBQd4nowKkLaY15RlEDT6DqRPj4FdhvQ7ZzTHM8QZNFPXiS3";
        private const string MerchantId = "tppf_chunyi_GP_POS_1";
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly string _connectionString;

        public PaymentService(
            HttpClient httpClient,
            ILogger<PaymentService> logger,
            TaipeiDbContext dbContext,
            IBookingRepository bookingRepository,
            IPaymentRepository paymentRepository,
            IConfiguration configuration
            )
        {
            _dbContext = dbContext;
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
            _httpClient = httpClient;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentDto paymentDto)
        {
            try
            {
                // 設置 HTTP Headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-api-key", PartnerKey);

                // 產生唯一的訂單編號 (年月日時分秒)
                string orderNumber = DateTime.Now.ToString("yyyyMMddHHmmss");

                // 準備付款資訊
                var paymentInfo = new
                {
                    prime = paymentDto.Prime,
                    partner_key = PartnerKey,
                    merchant_id = MerchantId,
                    order_number = orderNumber,
                    amount = paymentDto.Amount,
                    details = "台北一日遊",
                    cardholder = new
                    {
                        phone_number = paymentDto.Cardholder.PhoneNumber,
                        name = paymentDto.Cardholder.Name,
                        email = paymentDto.Cardholder.Email
                    },
                    remember = false
                };

                // 序列化為 JSON
                var content = new StringContent(
                    JsonSerializer.Serialize(paymentInfo),
                    Encoding.UTF8,
                    "application/json"
                );

                // 發送 POST 請求
                var response = await _httpClient.PostAsync(TapPayApiUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"TapPay Response: {responseContent}");

                // 解析 API 響應
                var paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // 處理成功的付款
                if (paymentResponse.Status == 0)
                {
                    _logger.LogInformation("Payment successful, processing booking and payment records.");
                    await ProcessSuccessfulPaymentAsync(paymentDto, orderNumber);
                }
                else
                {
                    _logger.LogError($"Payment failed with status: {paymentResponse.Status}, message: {paymentResponse.Message}");
                    throw new InvalidOperationException($"Payment failed: {paymentResponse.Message}");
                }

                paymentResponse.OrderNumber = orderNumber;

                return paymentResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Payment processing error: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessSuccessfulPaymentAsync(PaymentDto paymentDto, string orderNumber)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _logger.LogInformation($"Starting database transaction for order: {orderNumber}");

                        // 1. 更新預訂狀態 (使用共享的连接和事务)
                        var updatedBooking = await _bookingRepository.UpdateBookingStatusWithTransactionAsync(
                            paymentDto.UserId,
                            connection,
                            transaction);

                        if (updatedBooking == null)
                        {
                            _logger.LogWarning($"No active booking found for user: {paymentDto.UserId}");
                            throw new InvalidOperationException($"No active booking found for user: {paymentDto.UserId}");
                        }

                        // 2. 創建付款記錄 (使用共享的连接和事务)
                        await _paymentRepository.CreatePaymentWithTransactionAsync(
                            paymentDto,
                            updatedBooking,
                            orderNumber,
                            connection,
                            transaction);

                        // 提交事务
                        transaction.Commit();
                        _logger.LogInformation($"Database transaction committed successfully for order: {orderNumber}");
                    }
                    catch (Exception ex)
                    {
                        // 回滚事务
                        transaction.Rollback();
                        _logger.LogError(ex, $"Database transaction failed for order: {orderNumber}. Transaction rolled back.");
                        throw; // 重新抛出异常
                    }
                }
            }
        }
    }
}