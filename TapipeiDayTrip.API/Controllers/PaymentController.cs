using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application.Interfaces;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application.Services;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Requests;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.API.Controllers
{
    [Route("api/attraction/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger, IMapper mapper)
        {
            _paymentService = paymentService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            try
            {
                // 驗證請求
                if (request == null || string.IsNullOrEmpty(request.Prime))
                {
                    return BadRequest(new { error = true, message = "Invalid payment data" });
                }

                var paymentDto = _mapper.Map<PaymentDto>(request);
                // 處理支付
                var result = await _paymentService.ProcessPaymentAsync(paymentDto);

                // 檢查支付結果
                if (result.Status == 0) // 成功狀態碼
                {
                    return Ok(new
                    {
                        message = $"Payment successful: {result.Message}",
                        orderNumber = result.OrderNumber
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = $"Payment failed: {result.Message}"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment processing error: {Message}", ex.Message);

                // 加入更詳細的錯誤資訊
                _logger.LogError("Inner exception: {InnerException}", ex.InnerException?.Message);
                _logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);

                return StatusCode(500, new { error = true, message = "Internal server error occurred while processing payment" });
            }
        }
    }
}