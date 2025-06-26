using AutoMapper;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application.Interfaces;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Reponses;
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
        public async Task<BookingWithAttractionResponse> GetBookingByUserIdAsync(string id)
        {
            var bookingWithAttractionDto = await _bookingRepository.GetBookingByUserIdAsync(id);
            var imageUrl = GetFirstImageUrl(bookingWithAttractionDto.AttractionImages);
            var result = _mapper.Map<BookingWithAttractionResponse>(bookingWithAttractionDto);
            result.AttractionImages = imageUrl;
            return result;
        }
        public async Task<BookingWithAttractionResponse> CreateBookingAsync(BookingDto bookingDto)
        {
            var checkBookingPending = await _bookingRepository.GetBookingByUserIdAsync(bookingDto.UserId);
            if (checkBookingPending != null)
            {
                throw new Exception("You have a pending booking, please complete it first.");
            }
            bookingDto.CreatedAt = DateTime.UtcNow;
            bookingDto.UpdatedAt = DateTime.UtcNow;
            var bookingWithAttractionDto = await _bookingRepository.CreateBookingWithAttractionAsync(bookingDto);
            var imageUrl = GetFirstImageUrl(bookingWithAttractionDto.AttractionImages);
            var result = _mapper.Map<BookingWithAttractionResponse>(bookingWithAttractionDto);
            result.AttractionImages = imageUrl;
            return result;
        }
        public async Task<bool> DeleteBookingByUserIdAsync(string userId)
        {
            return await _bookingRepository.DeleteBookingByUserIdAsync(userId);
        }
        private string GetFirstImageUrl(string imageUrls)
        {
            if (string.IsNullOrWhiteSpace(imageUrls))
            {
                return null; // 或者你可以返回一個預設的圖片 URL
            }

            return imageUrls.Split(',')
                            .Select(url => url.Trim())
                            .FirstOrDefault();
        }
    }
}