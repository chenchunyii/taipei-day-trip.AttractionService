using Microsoft.AspNetCore.Mvc;
using taipei_day_trip_dotnet.TapipeiDayTrip.API.Reponse;
using TapipeiDayTrip.Application.Interfaces;
using AutoMapper;

namespace taipei_day_trip_dotnet.Controllers
{
    [Route("api/[controller]")]
    public class AttractionController : ControllerBase
    {
        private readonly IAttractionService _service;
        private readonly IMapper _mapper;

        public AttractionController(IAttractionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttractions()
        {
            try
            {
                var attractions = await _service.GetAllAttractionsAsync();
                return Ok(_mapper.Map<IList<AttractionsResponse>>(attractions));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}