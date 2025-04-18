using taipei_day_trip_dotnet.Models;

namespace taipei_day_trip_dotnet.Services
{
    public class AttractionServices : IAttractionService
    {
        private readonly IAttractionRepository _attractionRepository;
        public AttractionServices(IAttractionRepository attractionRepository)
        {
            _attractionRepository = attractionRepository;
        }
        public async Task<IList<AttractionModels>> GetAllAttractionsAsync()
        {
            return await _attractionRepository.GetAllAttractionsAsync();
        }
    }
}