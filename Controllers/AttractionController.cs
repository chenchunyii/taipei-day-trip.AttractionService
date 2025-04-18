using Microsoft.AspNetCore.Mvc;
using taipei_day_trip_dotnet.Services;

namespace taipei_day_trip_dotnet.Controllers
{
    [Route("api/[controller]")]
    public class AttractionController : ControllerBase
    {
        private readonly IAttractionService _service;

        public AttractionController(IAttractionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttractions()
        {
            try
            {
                var attractions = await _service.GetAllAttractionsAsync();
                return Ok(attractions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}