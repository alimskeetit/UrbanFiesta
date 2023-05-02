using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventViewModel createEventViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    error = ModelState.Values.SelectMany(v => v.Errors).ToList().Select(er => er.ErrorMessage),
                    createEventViewModel
                });
            var eve = _mapper.Map<Event>(createEventViewModel);
            await _eventRepository.CreateAsync(eve);
            return Ok(_mapper.Map<EventViewModel>(eve));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var events = await _eventRepository.GetAllAsync();
            return Ok(_mapper.Map<ICollection<EventViewModel>>(events));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var eve = await _eventRepository.GetByIdAsync(id);
            return Ok(_mapper.Map<EventViewModel>(eve));
        }

        [HttpPost("{eventId:int}")]
        public async Task<IActionResult> Like(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId, asTracking: true);
            var citizen = await _userManager.GetUserAsync(User);
            
            if (eve.Likes.Contains(citizen)) return Ok(_mapper.Map<EventViewModel>(eve));
            
            eve.Likes.Add(citizen);
            await _eventRepository.UpdateAsync(eve);
            return Ok(_mapper.Map<EventViewModel>(eve));
        }

        [HttpPost("{eventId:int}")]
        public async Task<IActionResult> RemoveLike(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId, asTracking: true);
            var citizen = await _userManager.GetUserAsync(User);
            eve.Likes.Remove(citizen);
            await _eventRepository.UpdateAsync(eve);
            return Ok(_mapper.Map<EventViewModel>(eve));
        }
    }
}
