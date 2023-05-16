using System.Diagnostics.Tracing;
using AutoMapper;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanFiesta.Filters;
using UrbanFiesta.Models.Event;
using UrbanFiesta.Repository;

namespace UrbanFiesta.Controllers
{
    [Route("[action]")]
    [Authorize(Roles="admin")]
    public class AdminController: ControllerBase
    {
        private readonly UserManager<Citizen> _userManager;
        private readonly EventRepository _eventRepository;
        private readonly IMapper _mapper;

        public AdminController(UserManager<Citizen> userManager, EventRepository eventRepository, IMapper mapper)
        {
            _userManager = userManager;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventViewModel createEventViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    error = ModelState.Values.SelectMany(v => v.Errors).ToList().Select(er => er.ErrorMessage),
                    createEventViewModel
                });
            var eve = _mapper.Map<Event>(createEventViewModel);
            if (eve.StartDate > (eve.EndDate ?? DateTime.MaxValue))
                return BadRequest(new
                {
                    error = "Дата начала не может быть позже окончания",
                    createEventViewModel
                });
            await _eventRepository.CreateAsync(eve);
            var vm = _mapper.Map<EventViewModel>(eve);
            foreach (var user in vm.Likes)
            {
                user.Roles = _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id)).GetAwaiter().GetResult().ToArray();
            }
            return Ok(vm);
        }

        [HttpPost("{email}")]
        public async Task<IActionResult> BanUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest($"Пользователь с email {email} не найден");
            user.IsBanned = true;
            await _userManager.UpdateAsync(user);

            return Ok($"Пользователь с email {email} забанен");
        }

        [HttpPost("{email}")]
        public async Task<IActionResult> UnbanUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest($"Пользователь с email {email} не найден");
            user.IsBanned = false;
            await _userManager.UpdateAsync(user);
            return Ok($"Пользователь с email {email} разбанен");
        }

        [HttpGet("{id:int}")]
        [Exist<Event>(pathToId: "eventId")]
        public async Task<IActionResult> UpdateEvent(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId, asTracking: true);
            return Ok(_mapper.Map<UpdateEventViewModel>(eve));
        }

        [HttpPut]
        [Exist<Event>(pathToId: "updateEventViewModel.Id")]
        public async Task<IActionResult> UpdateEvent([FromBody] CommandEventViewModel updateEventViewModel)
        {
            var eve = await _eventRepository.GetByIdAsync(updateEventViewModel.Id, asTracking: true);
            _mapper.Map(updateEventViewModel, eve);
            if (eve.StartDate > (eve.EndDate ?? DateTime.MaxValue))
                return BadRequest(new
                {
                    error = "Дата начала не может быть позже окончания",
                    updateEventViewModel
                });
            await _eventRepository.UpdateAsync(eve);
            var vm = _mapper.Map<EventViewModel>(eve);
            return Ok(vm);
        }

        [HttpPost("{eventId:int}")]
        [Exist<Event>(pathToId: "eventId")]
        public async Task<IActionResult> FinishEvent(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId, asTracking: true);
            eve.Status = EventStatus.Passed;
            eve.EndDate = DateTime.UtcNow;
            await _eventRepository.UpdateAsync(eve);
            var vm = _mapper.Map<EventViewModel>(eve);
            return Ok(vm);
        }

        [HttpDelete]
        [Exist<Event>(pathToId: "eventId")]
        public async Task<IActionResult> Delete(int eventId)
        {
            await _eventRepository.DeleteByIdAsync(eventId);
            return Ok();
        }
    }
}
