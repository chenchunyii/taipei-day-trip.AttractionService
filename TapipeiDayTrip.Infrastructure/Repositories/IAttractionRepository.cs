using taipei_day_trip_dotnet.Entity;

public interface IAttractionRepository
{
    Task<IList<AttractionEntity>> GetAllCategoriesAsync();
    Task<IList<AttractionEntity>> GetAttractionsAsync(int page, string? keyword);

    // Task<IList<AttractionEntity>> GetAttractionsAsync(int page, string keyword);
}
