using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application.Interfaces;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Requests;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.API.Controllers
{
    [Route("api/attraction/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;
        private readonly IMapper _mapper;

        public BookingController(IBookingService bookingService, IMapper mapper)
        {
            _service = bookingService;
            _mapper = mapper;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetBookingByUserId(string id)
        {
            var result = await _service.GetBookingByUserIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequest bookingRequest)
        {
            if (bookingRequest == null)
            {
                return BadRequest("Invalid booking data.");
            }

            var bookingDto = _mapper.Map<BookingDto>(bookingRequest);
            var result = await _service.CreateBookingAsync(bookingDto);
            if (result == null)
            {
                return BadRequest("Failed to create booking.");
            }

            return Ok(result);
        }
    }
}