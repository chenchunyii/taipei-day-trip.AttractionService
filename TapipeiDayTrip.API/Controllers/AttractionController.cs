using Microsoft.AspNetCore.Mvc;
using taipei_day_trip_dotnet.TapipeiDayTrip.API.Reponse;
using TapipeiDayTrip.Application.Interfaces;
using AutoMapper;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Reponse;

namespace taipei_day_trip_dotnet.Controllers
{
    [Route("api")]
    public class AttractionController : ControllerBase
    {
        private readonly IAttractionService _service;
        private readonly IMapper _mapper;

        public AttractionController(IAttractionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("attraction/categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var restul = await _service.GetAllCategoriesAsync();
                return Ok(_mapper.Map<IList<AttractionCategoryResponse>>(restul));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("attractions")]
        public async Task<IActionResult> GetAllAttractions([FromQuery] int page, [FromQuery] string? keyword)
        {
            try
            {
                var attractions = await _service.GetAttractionsAsync(page, keyword);
                return Ok(_mapper.Map<IList<AttractionsResponse>>(attractions));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}