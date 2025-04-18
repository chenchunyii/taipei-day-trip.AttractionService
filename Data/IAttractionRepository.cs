using taipei_day_trip_dotnet.Models;

public interface IAttractionRepository
{
    Task<IList<AttractionModels>> GetAllAttractionsAsync();
}
