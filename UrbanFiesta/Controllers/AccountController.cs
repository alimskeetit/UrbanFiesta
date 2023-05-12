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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<Citizen> userManager, SignInManager<Citizen> signInManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateCitizenViewModel createCitizenViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    error = ModelState.Values.SelectMany(v => v.Errors).ToList().Select(er => er.ErrorMessage),
                    createCitizenViewModel
                });
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

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCitizenViewModel loginCitizenViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values);
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut]
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

        [Authorize]
        [HttpPut]
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
            var vm = _mapper.Map<CitizenViewModel>(user);
            vm.Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToArray();
            return Ok(vm);
        }
    }
}
