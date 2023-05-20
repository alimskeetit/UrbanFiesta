using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UrbanFiesta.Filters;
using UrbanFiesta.Models.Citizen;
using UrbanFiesta.Services;

namespace UrbanFiesta.Controllers
{

    [Route("[action]")]
    public class PasswordController : ControllerBase
    {
        private readonly UserManager<Citizen> _userManager;
        private readonly EmailService _emailService;

        public PasswordController(UserManager<Citizen> userManager, EmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost]
        [ModelStateIsValid(model: "email")]
        public async Task<IActionResult> ForgotPassword([EmailAddress] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest($"Пользователя с почтой {email} не существует");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            _emailService.SendEmailAsyncByAdministration(
                toAddress: user.Email,
                subject: "Смена пароля",
                message:
                $"Для сброса пароля пройдите по ссылке: https://localhost:7099/ConfirmChangePassword?userEmail={user.Email}&token={Uri.EscapeDataString(token)}");
            return Ok(new [] {
                    "Письмо с ссылкой на смену пароля отправлено Вам на почту", token});
        }

        [AllowAnonymous]
        [HttpPost]
        [ModelStateIsValid(model: "resetPasswordViewModel")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            var result = await _userManager.ResetPasswordAsync(
                user,
                token: resetPasswordViewModel.Token,
                newPassword: resetPasswordViewModel.NewPassword);
            return result.Succeeded
                ? Ok()
                : BadRequest(new
                {
                    error = result.Errors.Select(error => error.Description),
                    resetPasswordViewModel
                });
        }

        [HttpPut]
        [ModelStateIsValid(model: "changePasswordViewModel")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel changePasswordViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(
                user,
                changePasswordViewModel.CurrentPassword,
                changePasswordViewModel.NewPassword);
            return result.Succeeded
                ? Ok()
                : BadRequest(new
                {
                    error = result.Errors.Select(error => error.Description),
                    changePasswordViewModel
                });
        }
    }
}
