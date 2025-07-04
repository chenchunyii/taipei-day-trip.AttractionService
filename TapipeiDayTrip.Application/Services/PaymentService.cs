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
                // 1. 參數驗證 - 增加詳細的驗證邏輯
                ValidatePaymentDto(paymentDto);

                // 2. 記錄請求開始和環境資訊
                _logger.LogInformation($"Starting payment process for user: {paymentDto.UserId}, Amount: {paymentDto.Amount}");
                _logger.LogInformation($"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

                // 3. 記錄當前 IP 資訊 (用於排查 IP mismatch 問題)
                await LogCurrentIpInformation();

                // 4. 驗證 TapPay 設定
                ValidateTapPayConfiguration();

                // 設置 HTTP Headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-api-key", PartnerKey);

                // 產生唯一的訂單編號 (加入毫秒確保唯一性)
                string orderNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff");

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

                // 記錄請求內容 (不包含敏感資訊)
                var logInfo = new
                {
                    merchant_id = MerchantId,
                    order_number = orderNumber,
                    amount = paymentDto.Amount,
                    cardholder_name = paymentDto.Cardholder.Name,
                    cardholder_email = paymentDto.Cardholder.Email
                };
                _logger.LogInformation($"Payment request info: {JsonSerializer.Serialize(logInfo)}");

                // 序列化為 JSON
                var content = new StringContent(
                    JsonSerializer.Serialize(paymentInfo),
                    Encoding.UTF8,
                    "application/json"
                );

                // 設置超時時間
                _httpClient.Timeout = TimeSpan.FromSeconds(30);

                // 發送 POST 請求
                _logger.LogInformation($"Sending request to TapPay API: {TapPayApiUrl}");
                var response = await _httpClient.PostAsync(TapPayApiUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"TapPay Response Status: {response.StatusCode}");
                _logger.LogInformation($"TapPay Response: {responseContent}");

                // 檢查 HTTP 狀態碼
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
                    throw new HttpRequestException($"TapPay API returned {response.StatusCode}: {response.ReasonPhrase}");
                }

                // 解析 API 響應
                PaymentResponse paymentResponse;
                try
                {
                    paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Failed to deserialize TapPay response: {ex.Message}");
                    _logger.LogError($"Response content: {responseContent}");
                    throw new InvalidOperationException("Invalid response format from TapPay API", ex);
                }

                if (paymentResponse == null)
                {
                    _logger.LogError("PaymentResponse is null after deserialization");
                    throw new InvalidOperationException("Failed to parse TapPay response");
                }

                // 處理成功的付款
                if (paymentResponse.Status == 0)
                {
                    _logger.LogInformation($"Payment successful for order: {orderNumber}, processing booking and payment records.");
                    await ProcessSuccessfulPaymentAsync(paymentDto, orderNumber);
                }
                else
                {
                    // 記錄具體的錯誤狀態
                    var errorMessage = GetTapPayErrorMessage(paymentResponse.Status);
                    _logger.LogError($"Payment failed with status: {paymentResponse.Status}, message: {paymentResponse.Message ?? "Unknown"}, description: {errorMessage}");

                    // 根據不同的錯誤狀態拋出不同的異常
                    if (paymentResponse.Status == 4) // IP mismatch
                    {
                        throw new InvalidOperationException($"IP mismatch error: {paymentResponse.Message}. Please check TapPay IP whitelist settings.");
                    }
                    else
                    {
                        throw new InvalidOperationException($"Payment failed (Status: {paymentResponse.Status}): {paymentResponse.Message ?? errorMessage}");
                    }
                }

                paymentResponse.OrderNumber = orderNumber;
                _logger.LogInformation($"Payment process completed successfully for order: {orderNumber}");

                return paymentResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error during payment processing");
                throw new InvalidOperationException("Network error occurred while processing payment", ex);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Payment request timeout");
                throw new InvalidOperationException("Payment request timeout", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Payment processing error: {ex.Message}");
                throw;
            }
        }

        private void ValidatePaymentDto(PaymentDto paymentDto)
        {
            if (paymentDto == null)
            {
                _logger.LogError("PaymentDto is null");
                throw new ArgumentNullException(nameof(paymentDto));
            }

            if (string.IsNullOrEmpty(paymentDto.Prime))
            {
                _logger.LogError("Prime is null or empty");
                throw new ArgumentException("Prime is required", nameof(paymentDto.Prime));
            }

            if (string.IsNullOrEmpty(paymentDto.UserId))
            {
                _logger.LogError("UserId is null or empty");
                throw new ArgumentException("UserId is required", nameof(paymentDto.UserId));
            }

            if (paymentDto.Amount <= 0)
            {
                _logger.LogError($"Invalid amount: {paymentDto.Amount}");
                throw new ArgumentException("Amount must be greater than 0", nameof(paymentDto.Amount));
            }

            if (paymentDto.Cardholder == null)
            {
                _logger.LogError("Cardholder information is null");
                throw new ArgumentException("Cardholder information is required", nameof(paymentDto.Cardholder));
            }

            if (string.IsNullOrEmpty(paymentDto.Cardholder.Name) ||
                string.IsNullOrEmpty(paymentDto.Cardholder.Email) ||
                string.IsNullOrEmpty(paymentDto.Cardholder.PhoneNumber))
            {
                _logger.LogError("Incomplete cardholder information");
                throw new ArgumentException("Complete cardholder information is required");
            }
        }

        private void ValidateTapPayConfiguration()
        {
            if (string.IsNullOrEmpty(PartnerKey))
            {
                _logger.LogError("TapPay PartnerKey is not configured");
                throw new InvalidOperationException("TapPay PartnerKey is not configured");
            }

            if (string.IsNullOrEmpty(MerchantId))
            {
                _logger.LogError("TapPay MerchantId is not configured");
                throw new InvalidOperationException("TapPay MerchantId is not configured");
            }

            if (string.IsNullOrEmpty(TapPayApiUrl))
            {
                _logger.LogError("TapPay API URL is not configured");
                throw new InvalidOperationException("TapPay API URL is not configured");
            }

            _logger.LogInformation($"TapPay Configuration - MerchantId: {MerchantId}, API URL: {TapPayApiUrl}");
        }

        private async Task LogCurrentIpInformation()
        {
            try
            {
                // 嘗試獲取外部 IP
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var externalIp = await client.GetStringAsync("https://api.ipify.org");
                _logger.LogInformation($"External IP: {externalIp}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to get external IP: {ex.Message}");
            }
        }

        private string GetTapPayErrorMessage(int status)
        {
            return status switch
            {
                1 => "參數錯誤",
                2 => "商店代號不存在",
                3 => "訂單重複",
                4 => "IP 限制",
                5 => "付款失敗",
                6 => "卡片錯誤",
                7 => "信用卡驗證失敗",
                8 => "餘額不足",
                9 => "付款被拒絕",
                10 => "交易未授權",
                _ => $"未知錯誤 (Status: {status})"
            };
        }

        private async Task ProcessSuccessfulPaymentAsync(PaymentDto paymentDto, string orderNumber)
        {
            _logger.LogInformation($"Starting database transaction for order: {orderNumber}");

            // 驗證資料庫連線字串
            if (string.IsNullOrEmpty(_connectionString))
            {
                _logger.LogError("Database connection string is null or empty");
                throw new InvalidOperationException("Database connection string is not configured");
            }

            try
            {
                using (IDbConnection connection = new MySqlConnection(_connectionString))
                {
                    _logger.LogInformation("Opening database connection");
                    await ((MySqlConnection)connection).OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            _logger.LogInformation($"Database connection opened successfully for order: {orderNumber}");

                            // 1. 檢查用戶是否有待付款的預訂
                            _logger.LogInformation($"Checking active booking for user: {paymentDto.UserId}");

                            // 2. 更新預訂狀態 (使用共享的連接和事務)
                            var updatedBooking = await _bookingRepository.UpdateBookingStatusWithTransactionAsync(
                                paymentDto.UserId,
                                connection,
                                transaction);

                            if (updatedBooking == null)
                            {
                                _logger.LogWarning($"No active booking found for user: {paymentDto.UserId}");
                                throw new InvalidOperationException($"No active booking found for user: {paymentDto.UserId}");
                            }

                            // 3. 創建付款記錄 (使用共享的連接和事務)
                            _logger.LogInformation($"Creating payment record for order: {orderNumber}");
                            await _paymentRepository.CreatePaymentWithTransactionAsync(
                                paymentDto,
                                updatedBooking,
                                orderNumber,
                                connection,
                                transaction);

                            // 提交事務
                            transaction.Commit();
                            _logger.LogInformation($"Database transaction committed successfully for order: {orderNumber}");
                        }
                        catch (Exception ex)
                        {
                            // 回滾事務
                            _logger.LogError(ex, $"Database transaction error for order: {orderNumber}, rolling back");
                            transaction.Rollback();
                            throw; // 重新拋出異常
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, $"MySQL database error during payment processing: {ex.Message}");
                throw new InvalidOperationException("Database error occurred during payment processing", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error during database transaction for order: {orderNumber}");
                throw;
            }
        }
    }
}