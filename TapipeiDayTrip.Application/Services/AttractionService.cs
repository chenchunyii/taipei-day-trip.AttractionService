using taipei_day_trip_dotnet.TapipeiDayTrip.API.Reponse;
using TapipeiDayTrip.Application.Interfaces;
using AutoMapper;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;

namespace taipei_day_trip_dotnet.Services
{
    public class AttractionServices : IAttractionService
    {
        private readonly IAttractionRepository _attractionRepository;
        private readonly IMapper _mapper;
        public AttractionServices(IAttractionRepository attractionRepository, IMapper mapper)
        {
            _attractionRepository = attractionRepository;
            _mapper = mapper;
        }
        public async Task<IList<AttractionDto>> GetAllAttractionsAsync()
        {
            var result = await _attractionRepository.GetAllAttractionsAsync();
            return _mapper.Map<IList<AttractionDto>>(result);
        }
    }
}