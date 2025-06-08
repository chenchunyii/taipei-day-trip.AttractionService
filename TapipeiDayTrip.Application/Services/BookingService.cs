using AutoMapper;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application.Interfaces;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Infrastructure.Repositories;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        public BookingService(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }
        public async Task<BookingDto> GetBookingByUserIdAsync(string id)
        {
            var result = await _bookingRepository.GetBookingByUserIdAsync(id);
            return _mapper.Map<BookingDto>(result);
        }
        public async Task<BookingDto> CreateBookingAsync(BookingDto bookingDto)
        {
            var result = await _bookingRepository.CreateBookingAsync(bookingDto);
            return _mapper.Map<BookingDto>(result);
        }
    }
}