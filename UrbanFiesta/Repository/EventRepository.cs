using Entities;
using Entities.Models;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace UrbanFiesta.Repository
{
    public class EventRepository: BaseRepository<Event>
    {
        public EventRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Event>> GetAllAsync(Expression<Func<Event, bool>>? predicate = null, bool asTracking = false)
        {
            return await base.GetAllAsync(predicate, asTracking, includeProperties: eve => eve.Likes);
        }

        public async Task<Event?> GetByIdAsync(int id, bool asTracking = false)
        {
            return await base.GetAsync(predicate: eve => id == eve.Id, asTracking, includeProperties: eve => eve.Likes);
        }
    }
}
