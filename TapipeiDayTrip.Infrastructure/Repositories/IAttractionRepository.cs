using taipei_day_trip_dotnet.Entity;

public interface IAttractionRepository
{
    Task<IList<AttractionEntity>> GetAllAttractionsAsync();
}
