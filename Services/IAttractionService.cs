using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taipei_day_trip_dotnet.Models;

namespace taipei_day_trip_dotnet.Services
{
    public interface IAttractionService
    {
        Task<IList<AttractionModels>> GetAllAttractionsAsync();
    }
}