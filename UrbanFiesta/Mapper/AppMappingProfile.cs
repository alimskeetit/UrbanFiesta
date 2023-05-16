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
        private readonly UserManager<Citizen> _userManager;

        public AppMappingProfile(AppDbContext context,
            UserManager<Citizen>? userManager)
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
                        eve.Coordinates.Split(new[] { '|' }).Select(coord => double.Parse(coord)).ToArray()))
                .ReverseMap()
                .ForMember(eve => eve.Coordinates,
                    opt => opt.MapFrom(eveModel => string.Join('|', eveModel.Coordinates)));

            //CreateMap<Event, EventViewModelIgnoreLikes>()
            //    .ForMember(eveModel => eveModel.Coordinates,
            //        opt => opt.Ignore())
            //    .ReverseMap();
                //.ForMember(eve => eve.Coordinates,
                //    opt => opt.MapFrom(eveModel => string.Join('|', eveModel.Coordinates)));

                CreateMap<Event, CommandEventViewModel>()
                    .ReverseMap()
                    .ForMember(eve => eve.EndDate,
                        opt => opt.MapFrom(commandEve =>
                            commandEve.EndDate == string.Empty ? null : commandEve.EndDate))
                    .ForMember(eve => eve.Coordinates,
                        opt => opt.MapFrom(commandEve => string.Join('|', commandEve.Coordinates)))
                    .ForMember(eve => eve.Status,
                        opt => opt.MapFrom(commandEve => EventStatus.NotStarted))
                    .ReverseMap()
                    .ForMember(commandEve => commandEve.Coordinates,
                        opt => opt.MapFrom(eve =>
                            eve.Coordinates.Split(new[] { '|' }).Select(coord => double.Parse(coord)).ToArray()));
                    

                CreateMap<Event, UpdateEventViewModel>()
                    .ForMember(commandEve => commandEve.Coordinates,
                        opt => opt.MapFrom(eve =>
                            eve.Coordinates.Split(new[] { '|' }).Select(coord => double.Parse(coord)).ToArray()));
                //CreateMap<Event, CreateEventViewModel>()
            //    .ReverseMap()
            //    .ForMember(eve => eve.EndDate,
            //        opt => opt.MapFrom(createEve => createEve.EndDate == string.Empty ? null : createEve.EndDate));
            //CreateMap<Event, UpdateEventViewModel>().ReverseMap()
            //    .ForMember(eve => eve.EndDate,
            //        opt => opt.MapFrom(createEve => createEve.EndDate == string.Empty ? null : createEve.EndDate));
        }

        private string[] Expression(Citizen citizen)
        {
            return _userManager.GetRolesAsync(citizen).GetAwaiter().GetResult().ToArray();
        }
    }
}
