using System.Runtime.CompilerServices;
using Entities;
using Entities.Enums;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace UrbanFiesta.Services
{
    public class EventReminder : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private EmailService _emailService;
        private AppDbContext _context;
        private Timer _timer;

        public EventReminder(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendNotificationAboutEventOneDayBefore, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void SendNotificationAboutEventOneDayBefore(object state)
        {
            using var scope = _scopeFactory.CreateScope();
            _emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
            _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var citizenAndLikedEvents = TakeCitizenAndTheirLikedEvents(_context);
            if (!citizenAndLikedEvents.Any())
                return;
            var emails = citizenAndLikedEvents.Select(citizenEvent => new object[]
                {
                    citizenEvent.Citizen.EmailForNewsletter,
                    $"Здравствуйте, {citizenEvent.Citizen.FirstName}! " +
                    $"Уведомляем Вас о том, что скоро начнется мероприятие {citizenEvent.Event.Title}, " +
                    $"которое пройдет {citizenEvent.Event.StartDate.Date} в {citizenEvent.Event.StartDate.TimeOfDay} " +
                    $"по адресу {citizenEvent.Event.Address}"
                }
            ).ToArray();
            _emailService
                .SendEmailsAsyncByAdministration(emails, "Уведомление о мероприятии")
                .GetAwaiter();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
            return Task.CompletedTask;
        }

        private IQueryable<CitizenEvent> TakeCitizenAndTheirLikedEvents(AppDbContext context)
        {
            var events = context.Events.Where(eve =>
                    (DateTime.Now - eve.StartDate).TotalDays <= 1 && eve.Status != EventStatus.Passed
                );
            var recipients =
                context.Users
                    .Where(user => user.IsSubscribed && !user.IsBanned)
                    .SelectMany(user => user.LikedEvents
                        .Where(likedEve => events.Contains(likedEve))
                        .Select(eve => new CitizenEvent
                        {
                            Citizen = user,
                            Event = eve
                        }));
            return recipients;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }

    public struct CitizenEvent
    {
        public Citizen Citizen;
        public Event Event;
    }
}
