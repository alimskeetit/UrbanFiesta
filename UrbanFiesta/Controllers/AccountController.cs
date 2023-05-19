using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UrbanFiesta.Filters;
using UrbanFiesta.Models;
using UrbanFiesta.Models.Citizen;
using UrbanFiesta.Services;

namespace UrbanFiesta.Controllers
{
    [Authorize]
    [Route("[action]")]
    public class AccountController: ControllerBase
    {
        private readonly UserManager<Citizen> _userManager;
        private readonly SignInManager<Citizen> _signInManager;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<Citizen> userManager, SignInManager<Citizen> signInManager, IMapper mapper, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost]
        [ModelStateIsValid(model: "createCitizenViewModel")]
        public async Task<IActionResult> Register([FromBody] CreateCitizenViewModel createCitizenViewModel)
        {
            var user = _mapper.Map<Citizen>(createCitizenViewModel);
            var result = await _userManager.CreateAsync(user, createCitizenViewModel.Password);
            if (!result.Succeeded)
                return BadRequest(new
                {
                    error = result.Errors.Select(error => error.Description),
                    createCitizenViewModel
                });
            await _userManager.AddToRoleAsync(user, "user");
            await _signInManager.SignInAsync(user, isPersistent: false);
            var vm = _mapper.Map<CitizenViewModel>(user);
            vm.Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToArray();
            return Ok(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ModelStateIsValid(model: "loginCitizenViewModel")]
        public async Task<IActionResult> Login([FromBody] LoginCitizenViewModel loginCitizenViewModel)
        {
            var user = await _userManager.FindByNameAsync(loginCitizenViewModel.Email);
            if (user == null) return BadRequest("Неправильный логин или пароль");
            var result = await _signInManager.PasswordSignInAsync(
                userName: loginCitizenViewModel.Email,
                password: loginCitizenViewModel.Password,
                isPersistent: loginCitizenViewModel.RememberMe,
                lockoutOnFailure: false);
            var vm = _mapper.Map<CitizenViewModel>(user);
            vm.Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToArray();
            return result.Succeeded ? 
                Ok(vm) 
                : BadRequest("Неверный логин или пароль");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = _mapper.Map<EditCitizenViewModel>(user);
            return Ok(vm);
        }

        [HttpPut]
        [ModelStateIsValid(model: "editCitizenViewModel")]
        public async Task<IActionResult> Edit([FromBody] EditCitizenViewModel editCitizenViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            _mapper.Map(editCitizenViewModel, user);
            user.UserName = user.Email;
            var result = await _userManager.UpdateAsync(user!);
            if (!result.Succeeded)
                return BadRequest(new
                {
                    error = result.Errors.Select(error => error.Description),
                    editCitizenViewModel
                });
            await _signInManager.RefreshSignInAsync(user!);
            return Ok(editCitizenViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = _mapper.Map<CitizenViewModel>(user);
            vm.Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToArray();
            return Ok(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SubscribeToNewsletter([FromBody] EmailViewModel emailForNewsLetter)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _userManager.GetUserAsync(User);
            if (emailForNewsLetter.Email == user.Email && user.EmailConfirmed)
            {
                user.IsSubscribed = true;
                return Ok("Вы подписаны на рассылку");
            }
            var code = new Random().Next(100000, 999999);
            user.CodeForConfirmEmailForNewsletter = code.ToString();
            await _userManager.UpdateAsync(user);
            await _emailService.SendEmailAsyncByAdministration(
                toAddress: emailForNewsLetter.Email,
                subject: "Подтверждение подписки на рассылку",
                message: $"Код подтверждения этой почты для получения рассылок: {code}");
            return Ok($"Письмо с кодом для подтверждения отправлено на почту {emailForNewsLetter.Email}");
        }

        [HttpPost]
        public async Task<IActionResult> FinalSubToNewsletter([FromBody] FinalSubToNewsLetterViewModel emailForNewsLetter)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.CodeForConfirmEmailForNewsletter != emailForNewsLetter.Code)
                return BadRequest("Неверный код");
            user.EmailForNewsletter = emailForNewsLetter.Email;
            user.IsSubscribed = true;
            await _userManager.UpdateAsync(user);
            return Ok("Вы подписаны на рассылку");
        }

        [HttpPost]
        public async Task<IActionResult> UnsubscribeToNewsletter()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsSubscribed = false;
            await _userManager.UpdateAsync(user);
            return Ok("Вы отписаны от рассылки");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            var token = _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendEmailAsyncByAdministration(
                toAddress: user.Email,
                subject: "Подтверждение электронной почты",
                message:
                $"ЭТО ТЕСТ ПРИЛОЖЕНИЯ, ЭТО НЕ СПАМ!!! Подтвердите электронную почту от аккаунта на {Environment.GetEnvironmentVariable("ORIGIN") ?? "supersite.com"}, " +
                $"перейдя по ссылке: " +
                $"https://localhost:7099/FinalConfirmEmail?userId={user.Id}&token={Uri.EscapeDataString(token.Result)}");
            return Ok("Письмо с ссылкой на подтверждение почты отправлено Вам на почту");
        }

        [HttpGet] 
        [Exist<Citizen>(pathToId: "userId")]
        public async Task<IActionResult> FinalConfirmEmail([FromBody]FinalConfirmEmailViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.UserId);
            var result = await _userManager.ConfirmEmailAsync(user, viewModel.Token);
            return result.Succeeded ? Ok("Почта подтверждена") : BadRequest("Ошибка");
        }
    }
}
