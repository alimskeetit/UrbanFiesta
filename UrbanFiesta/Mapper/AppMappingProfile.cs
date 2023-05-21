using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AutoMapper;
using Entities;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            CreateMap<Citizen, LoginCitizenViewModel>()
                .ReverseMap();
            CreateMap<Citizen, CreateCitizenViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<Citizen, CitizenViewModelIgnoreLikedEvents>()
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<Citizen, EditCitizenViewModel>()
                .ReverseMap();

            CreateMap<Event, EventViewModel>()
                .ForMember(eveModel => eveModel.Coordinates,
                    opt => opt.MapFrom(eve =>
                        eve.Coordinates.Split(new[] { '|' })))
                .ReverseMap()
                .ForMember(eve => eve.Coordinates,
                    opt => opt.MapFrom(eveModel => string.Join('|', eveModel.Coordinates)));

            CreateMap<Event, CommandEventViewModel>()
                    .ForMember(commandEve => commandEve.Coordinates,
                        opt => opt.MapFrom(eve =>
                            eve.Coordinates.Split(new[] { '|' })))
                    .ReverseMap()
                    .ForMember(eve => eve.EndDate,
                        opt => opt.MapFrom(commandEve =>
                            commandEve.EndDate == string.Empty ? null : commandEve.EndDate))
                    .ForMember(eve => eve.Coordinates,
                        opt => opt.MapFrom(commandEve => string.Join('|', commandEve.Coordinates)))
                    .ForMember(eve => eve.Status,
                        opt => opt.MapFrom(commandEve => EventStatus.NotStarted));

                CreateMap<Event, UpdateEventViewModel>()
                    .ForMember(commandEve => commandEve.Coordinates,
                        opt => opt.MapFrom(eve =>
                            eve.Coordinates.Split(new[] { '|' })));
        }
    }
}
