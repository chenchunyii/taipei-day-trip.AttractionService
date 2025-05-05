using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;

namespace TapipeiDayTrip.Application.Interfaces
{
    public interface IAttractionService
    {
        Task<IList<AttractionCategoryDto>> GetAllCategoriesAsync();
        Task<IList<AttractionDto>> GetAttractionsAsync(int page, string? keyword);
        Task<AttractionDto> GetAttractionByIdAsync(int id);
    }
}