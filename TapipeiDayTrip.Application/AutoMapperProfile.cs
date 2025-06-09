using AutoMapper;
using taipei_day_trip_dotnet.Entity;
using taipei_day_trip_dotnet.TapipeiDayTrip.API.Reponse;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.DTOs;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Entities;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Reponse;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Reponses;
using taipei_day_trip_dotnet.TapipeiDayTrip.Domain.Requests;

namespace taipei_day_trip_dotnet.TapipeiDayTrip.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Attraction, AttractionDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                    src.Images != null
                    ? src.Images.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    : null
                    ));
            CreateMap<Attraction, AttractionCategoryDto>();

            CreateMap<AttractionDto, AttractionsResponse>();
            CreateMap<AttractionCategoryDto, AttractionCategoryResponse>();
            CreateMap<BookingRequest, BookingDto>();
            CreateMap<Booking, BookingDto>();
            CreateMap<BookingWithAttractionDto, BookingWithAttractionResponse>();
        }
    }
}