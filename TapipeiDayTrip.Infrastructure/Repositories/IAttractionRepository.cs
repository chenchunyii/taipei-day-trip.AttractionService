using taipei_day_trip_dotnet.Entity;

public interface IAttractionRepository
{
    Task<IList<Attraction>> GetAllCategoriesAsync();
    Task<IList<Attraction>> GetAttractionsAsync(int page, string? keyword);
    Task<Attraction> GetAttractionByIdAsync(int id);
}
