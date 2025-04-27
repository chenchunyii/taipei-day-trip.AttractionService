using AutoMapper;
using taipei_day_trip_dotnet.Entity;
using taipei_day_trip_dotnet.TapipeiDayTrip.API.Reponse;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Reponse;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AttractionEntity, AttractionDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => 
                    src.Images != null 
                    ? src.Images.Split(',', StringSplitOptions.RemoveEmptyEntries) 
                    : null
                    ));
            CreateMap<AttractionEntity, AttractionCategoryDto>();
            
            CreateMap<AttractionDto, AttractionsResponse>();
            CreateMap<AttractionCategoryDto, AttractionCategoryResponse>();
        }
    }
}