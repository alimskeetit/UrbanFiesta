using Entities;
using Entities.Models;
using Microsoft.Identity.Client;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;

namespace UrbanFiesta.Repository
{
    public class EventRepository: BaseRepository<Event>
    {
        private readonly UserManager<Citizen> _userManager;
        public EventRepository(AppDbContext context, UserManager<Citizen> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<ICollection<Event>> GetAllAsync(Expression<Func<Event, bool>>? predicate = null, bool asTracking = false)
        {
            var eves =  await base.GetAllAsync(predicate, asTracking, includeProperties: eve => eve.Likes);
            foreach (var eve in eves)
            {
                await SetRoles(eve);
            }
            return eves;
        }

        public async Task<Event?> GetByIdAsync(int id, bool asTracking = false)
        {
            var eve = await base.GetAsync(predicate: eve => id == eve.Id, asTracking, includeProperties: eve => eve.Likes);
            await SetRoles(eve);
            return eve;
        }

        public async Task DeleteByIdAsync(int id)
        {
            await base.DeleteAsync(eve => id == eve.Id);
        }

        private async Task SetRoles(Event eve)
        {
            foreach (var citizen in eve.Likes) 
                citizen.Roles = (await _userManager.GetRolesAsync(citizen)).ToArray();
        }
    }
}
