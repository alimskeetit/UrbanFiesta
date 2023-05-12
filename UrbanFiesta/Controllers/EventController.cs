using AutoMapper;
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
        public async Task<IActionResult> Get()
        {
            var events = await _eventRepository.GetAllAsync();
            var vms = _mapper.Map<ICollection<EventViewModel>>(events);
            foreach (var vm in vms)
            {
                foreach (var user in vm.Likes)
                {
                    user.Roles = _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id)).GetAwaiter().GetResult().ToArray();
                }
            }
            return Ok(vms);
        }

        [HttpGet("{id:int}")]
        [Exist<Event>]
        public async Task<IActionResult> Get(int id)
        {
            var eve = await _eventRepository.GetByIdAsync(id);
            var vm = _mapper.Map<EventViewModel>(eve);
            foreach (var user in vm.Likes)
            {
                user.Roles = _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id)).GetAwaiter().GetResult().ToArray();
            }
            return Ok(vm);
        }

        [Authorize(Policy = "NotBanned")]
        [HttpPost("{eventId:int}")]
        [Exist<Event>(pathToId: "eventId")]
        public async Task<IActionResult> Like(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId, asTracking: true);
            var citizen = await _userManager.GetUserAsync(User);
            eve.Likes.Add(citizen);
            await _eventRepository.UpdateAsync(eve);
            var vm = _mapper.Map<EventViewModel>(eve);
            foreach (var user in vm.Likes)
            {
                user.Roles = _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id)).GetAwaiter().GetResult().ToArray();
            }
            return Ok(vm);
        }

        [HttpPost("{eventId:int}")]
        [Exist<Event>(pathToId: "eventId")]
        public async Task<IActionResult> RemoveLike(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId, asTracking: true);
            var citizen = await _userManager.GetUserAsync(User);
            eve.Likes.Remove(citizen);
            await _eventRepository.UpdateAsync(eve);
            var vm = _mapper.Map<EventViewModel>(eve);
            foreach (var user in vm.Likes)
            {
                user.Roles = _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id)).GetAwaiter().GetResult().ToArray();
            }
            return Ok(vm);
        }
    }
}
