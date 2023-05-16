using Entities;
using Entities.Enums;
using Entities.Models;

namespace UrbanFiesta.Services
{
    public class EventsStatusUpdateService: IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private AppDbContext _context;
        private Timer _timer;


        public EventsStatusUpdateService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateEventStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)); // Обновление каждые 10 минут
            return Task.CompletedTask;
        }

        private void UpdateEventStatus(object state)
        {
            using var scope = _scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currentTime = DateTime.UtcNow.ToLocalTime();
            var eventsStarted = _context.Events
                .Where(eve => eve.StartDate <= currentTime && eve.EndDate != null).ToList();
            foreach (var eve in eventsStarted)
            {
                eve.Status = EventStatus.Started;
            }

            var eventsPassed = eventsStarted
                .Where(eve => eve.EndDate <= currentTime);
            foreach (var eve in eventsPassed)
            {
                eve.Status = EventStatus.Passed;
            }


            _context.SaveChanges();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
