using Microsoft.EntityFrameworkCore;
using taipei_day_trip_dotnet.Models;

namespace taipei_day_trip_dotnet.Data
{
    public class AttractionRepository : IAttractionRepository
    {
        private readonly TaipeiDbContext _context;

        public AttractionRepository(TaipeiDbContext context)
        {
            _context = context;
        }

        public async Task<IList<AttractionModels>> GetAllAttractionsAsync()
        {
            var result =  await _context.Attractions.ToListAsync();
            return result;
        }
    }
    
}