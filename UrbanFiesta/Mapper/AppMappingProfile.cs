using System.Globalization;
using System.Runtime.CompilerServices;
using AutoMapper;
using Entities;
using Entities.Models;
using UrbanFiesta.Models.Citizen;
using UrbanFiesta.Models.Event;

namespace UrbanFiesta.Mapper
{
    public class AppMappingProfile: Profile
    {
        private readonly AppDbContext _context;

        public AppMappingProfile(AppDbContext context)
        {
            _context = context;
            CreateMap<Citizen, CitizenViewModel>()
                .ForMember(dest => dest.LikedEvents, opt => opt.MapFrom(src => _context.Entry(src).Collection(c => c.LikedEvents).Query().ToList()))
                .ReverseMap();
            CreateMap<Citizen, LoginCitizenViewModel>().ReverseMap();
            CreateMap<Citizen, CreateCitizenViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                /*.ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(TimeOnly.MinValue)))*/;
            CreateMap<Citizen, CitizenViewModelIgnoreLikedEvents>()
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<Citizen, EditCitizenViewModel>()
                .ReverseMap();
            CreateMap<Event, EventViewModel>().ReverseMap();
            CreateMap<Event, EventViewModelIgnoreLikes>().ReverseMap();
            CreateMap<Event, CreateEventViewModel>()
                .ReverseMap()
                .ForMember(eve => eve.EndDate,
                    opt => opt.MapFrom(createEve => createEve.EndDate == string.Empty ? null : createEve.EndDate));
        }

        public AppMappingProfile()
        {
            CreateMap<Citizen, CitizenViewModel>().ReverseMap();
            CreateMap<Citizen, LoginCitizenViewModel>().ReverseMap();
        }
    }
}
