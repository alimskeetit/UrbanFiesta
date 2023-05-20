
using AutoMapper;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UrbanFiesta.Filters;
using UrbanFiesta.Models.Citizen;
using UrbanFiesta.Models.Event;
using UrbanFiesta.Repository;

namespace UrbanFiesta.Controllers
{
    [Route("[action]")]
    public class EventController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EventRepository _eventRepository;
        private readonly UserManager<Citizen> _userManager;

        public EventController(IMapper mapper, EventRepository eventRepository, UserManager<Citizen> userManager)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents(int count = 0, int page = 0, bool sortByDate = true)
        {
            if (count < 0) return BadRequest("count должен быть >= 0");
            if (page == 0) page = 1;
            var events = await _eventRepository.GetAllAsync();
            if (sortByDate)
                events = events.OrderBy(eve => eve.StartDate).ToList();
            if (page < 0)
                events = events.Reverse().ToList();
            var vms = _mapper.Map<ICollection<EventViewModel>>(events
                .Skip((int.Abs(page) - 1) * count)
                .Take(count).ToList());
            return Ok(vms);
        }

        [HttpGet]
        [Exist<Event>(pathToId: "eventId")]
        public async Task<IActionResult> GetEvent(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId);
            var vm = _mapper.Map<EventViewModel>(eve);
            return Ok(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetEventByDate(DateTime date, bool sortByLikes = false)
        {
            var events = await _eventRepository.GetAllAsync(eve => eve.StartDate.Date == date);
            if (sortByLikes)
                events = events.OrderByDescending(eve => eve.Likes.Count).ToList();
            var vms = _mapper.Map<ICollection<EventViewModel>>(events);
            return Ok(vms);
        }

        [HttpGet]
        public async Task<IActionResult> GetTopEvents(int count)
        {
            if (count < 0) 
                return BadRequest("count должен быть больше либо равен 0");

            var events = await _eventRepository.GetAllAsync(eve => eve.Status != EventStatus.Passed);
            events = events.OrderByDescending(eve => eve.Likes.Count).Where(eve => eve.Likes.Count != 0).Take(count).ToList();
            var vms = _mapper.Map<ICollection<EventViewModel>>(events);
            return Ok(vms);
        }

        [Authorize(Policy = "NotBanned")]
        [HttpPost]
        [Exist<Event>(pathToId: "eventId")]
        public async Task<IActionResult> LikeEvent(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId, asTracking: true);
            var citizen = await _userManager.GetUserAsync(User);
            citizen.Roles = (await _userManager.GetRolesAsync(citizen)).ToArray();
            eve.Likes.Add(citizen);
            await _eventRepository.UpdateAsync(eve);
            var vm = _mapper.Map<EventViewModel>(eve);
            return Ok(vm);
        }

        [Authorize(Policy = "NotBanned")]
        [HttpPost]
        [Exist<Event>(pathToId: "eventId")]
        public async Task<IActionResult> RemoveLikeFromEvent(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId, asTracking: true);
            var citizen = await _userManager.GetUserAsync(User);
            eve.Likes.Remove(citizen);
            await _eventRepository.UpdateAsync(eve);
            var vm = _mapper.Map<EventViewModel>(eve);
            return Ok(vm);
        }
    }
}
