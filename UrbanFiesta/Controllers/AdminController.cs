﻿using System.Diagnostics.Tracing;
using AutoMapper;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbanFiesta.Filters;
using UrbanFiesta.Models;
using UrbanFiesta.Models.Event;
using UrbanFiesta.Repository;
using UrbanFiesta.Services;

namespace UrbanFiesta.Controllers
{
    [Authorize(Roles="admin")]
    [Route("[action]")]
    public class AdminController: ControllerBase
    {
        private readonly UserManager<Citizen> _userManager;
        private readonly EventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;

        public AdminController(UserManager<Citizen> userManager, EventRepository eventRepository, IMapper mapper, EmailService emailService)
        {
            _userManager = userManager;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        [HttpPost("")]
        [ModelStateIsValid(model: "createEventViewModel")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventViewModel createEventViewModel)
        {
            var eve = _mapper.Map<Event>(createEventViewModel);
            if (eve.StartDate > (eve.EndDate ?? DateTime.MaxValue))
                return BadRequest(new
                {
                    error = "Дата начала не может быть позже окончания",
                    createEventViewModel
                });
            await _eventRepository.CreateAsync(eve);
            var vm = _mapper.Map<EventViewModel>(eve);
            return Ok(vm);
        }

        [HttpGet]
        [Exist<Event>(pathToId: "eventId")]
        public async Task<IActionResult> UpdateEvent(int eventId)
        {
            var eve = await _eventRepository.GetByIdAsync(eventId, asTracking: true);
            return Ok(_mapper.Map<UpdateEventViewModel>(eve));
        }

        [HttpPut]
        [ModelStateIsValid(model: "updateEventViewModel"), 
         Exist<Event>(pathToId: "updateEventViewModel.Id")]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventViewModel updateEventViewModel)
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

        [HttpPost]
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
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            await _eventRepository.DeleteByIdAsync(eventId);
            return Ok();
        }

        [HttpPost]
        public async Task SendNewsletterToCitizens([FromBody] MessageToCitizens messageToCitizens)
        {
            await _emailService.SendEmailToSubscribersByAdministrationAsync(
                subject: messageToCitizens.Subject,
                message: messageToCitizens.Message,
                emails: messageToCitizens.Emails);
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest($"Пользователь с email {email} не найден");
            if ((await _userManager.GetRolesAsync(user)).Contains("admin"))
                return BadRequest("Нельзя заблокировать администратора");
            user.IsBanned = true;
            await _userManager.UpdateAsync(user);

            return Ok($"Пользователь с email {email} забанен");
        }

        [HttpPost]
        public async Task<IActionResult> UnbanUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest($"Пользователь с email {email} не найден");
            user.IsBanned = false;
            await _userManager.UpdateAsync(user);
            return Ok($"Пользователь с email {email} разбанен");
        }
    }
}
