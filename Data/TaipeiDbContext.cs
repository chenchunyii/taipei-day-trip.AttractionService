using Microsoft.EntityFrameworkCore;
using taipei_day_trip_dotnet.Models;

namespace taipei_day_trip_dotnet.Data
{
    public class TaipeiDbContext : DbContext
    {
         public TaipeiDbContext(DbContextOptions<TaipeiDbContext> options) : base(options) { }

    public DbSet<TaipeiModels> Members => Set<TaipeiModels>();
    }
}