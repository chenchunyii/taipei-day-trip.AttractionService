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
        public async Task<IList<AttractionCategoryDto>> GetAllCategoriesAsync()
        {
            var restul = await _attractionRepository.GetAllCategoriesAsync();
            return _mapper.Map<IList<AttractionCategoryDto>>(restul);
        }
        public async Task<IList<AttractionDto>> GetAttractionsAsync(int page, string? keyword)
        {
            var result = await _attractionRepository.GetAttractionsAsync(page, keyword);
            return _mapper.Map<IList<AttractionDto>>(result);
        }
    }
}