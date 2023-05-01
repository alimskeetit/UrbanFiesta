using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UrbanFiesta.Models.Citizen;

namespace UrbanFiesta.Controllers
{
    [Route("[action]")]
    public class AccountController: ControllerBase
    {
        private readonly UserManager<Citizen> _userManager;
        private readonly SignInManager<Citizen> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<Citizen> userManager, SignInManager<Citizen> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateCitizenViewModel createCitizenViewModel)
        {
            var user = _mapper.Map<Citizen>(createCitizenViewModel);
            var result = await _userManager.CreateAsync(user, createCitizenViewModel.Password);
            if (!result.Succeeded) return BadRequest(
                new
                {
                    error = result.Errors.Select(error => error.Description),
                    createCitizenViewModel
                });
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(_mapper.Map<CitizenViewModelIgnoreLikedEvents>(user));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCitizenViewModel loginCitizenViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
            var result = await _signInManager.PasswordSignInAsync(
                userName: loginCitizenViewModel.Email,
                password: loginCitizenViewModel.Password,
                isPersistent: loginCitizenViewModel.RememberMe,
                lockoutOnFailure: false);
            var user = await _userManager.FindByNameAsync(loginCitizenViewModel.Email);
            return result.Succeeded ? Ok(_mapper.Map<CitizenViewModelIgnoreLikedEvents>(user)) : BadRequest("Неверный логин или пароль");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var editCitizenViewModel = _mapper.Map<EditCitizenViewModel>(user);
            return Ok(editCitizenViewModel);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditCitizenViewModel editCitizenViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            _mapper.Map(editCitizenViewModel, user);
            user.UserName = user.Email;
            var result = await _userManager.UpdateAsync(user!);
            if (!result.Succeeded) return BadRequest(
                new
                {
                    error = result.Errors.Select(error => error.Description),
                    editCitizenViewModel
                });
            await _signInManager.RefreshSignInAsync(user!);
            return Ok(editCitizenViewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return Ok(new ChangePasswordViewModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel changePasswordViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(
                user: user,
                currentPassword: changePasswordViewModel.CurrentPassword,
                newPassword: changePasswordViewModel.NewPassword);
            if (result.Succeeded) return Ok();
            return BadRequest(new
            {
                error = result.Errors.Select(error => error.Description),
                changePasswordViewModel
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> About()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(_mapper.Map<CitizenViewModelIgnoreLikedEvents>(user));
        }
    }
}
