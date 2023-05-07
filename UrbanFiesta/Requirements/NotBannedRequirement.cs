using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace UrbanFiesta.Requirements
{
    public class NotBannedHandler: AuthorizationHandler<NotBannedRequirement>
    {
        private readonly UserManager<Citizen> _userManager;

        public NotBannedHandler(UserManager<Citizen> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            NotBannedRequirement requirement)
        {
            var citizen = await _userManager.GetUserAsync(context.User);
            if (!citizen.IsBanned)
                context.Succeed(requirement);
        }
    }

    public class NotBannedRequirement : IAuthorizationRequirement
    {
    }
}
