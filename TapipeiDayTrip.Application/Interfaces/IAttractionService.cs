using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;

namespace TapipeiDayTrip.Application.Interfaces
{
    public interface IAttractionService
    {
        Task<IList<AttractionDto>> GetAllAttractionsAsync();
    }
}