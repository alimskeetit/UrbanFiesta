using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

namespace UrbanFiesta.Services
{
    public class EmailService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private AppDbContext _context;
        private Timer _timer;

        public EmailService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        public async Task SendEmailAsync(string fromName, 
            string fromAddress,
            string password,
            string toAddress, 
            string subject, 
            string message)
        {
            using var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(fromName, fromAddress));
            emailMessage.To.Add(new MailboxAddress(string.Empty, toAddress));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text =  message
            };
            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.yandex.ru", 465, true);
            await client.AuthenticateAsync(fromAddress, password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailsAsync(string fromName,
            string fromAddress,
            string password, 
            object[][] emails,
            string subject)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.yandex.ru", 465, true);
            await client.AuthenticateAsync(fromAddress, password);
            for (var i = 0; i < emails.GetLength(0); i++)
            {
                using var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(fromName, fromAddress));
                emailMessage.To.Add(new MailboxAddress("", address: (string)emails[i][0]));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = (string)emails[i][1]
                };
                await client.SendAsync(emailMessage);
            }
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailAsyncByAdministration(string toAddress, string subject, string message)
        {
            await SendEmailAsync(
                fromName: "Администрация сайта",
                fromAddress: "ivanalimsky@yandex.ru",
                password: "szbfzltgzoypdlgy",
                toAddress: toAddress,
                subject: subject,
                message: message);
        }

        public async Task SendEmailsAsyncByAdministration(object[][] emails, string subject)
        {
            await SendEmailsAsync(
                fromName: "Администрация сайта",
                fromAddress: "ivanalimsky@yandex.ru",
                password: "szbfzltgzoypdlgy",
                emails: emails,
                subject: subject);
        }

        public async Task SendEmailToSubscribersByAdministrationAsync(string subject, string message, string[]? emails = null)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await SendEmailsAsyncByAdministration(
                emails: context.Users
                    .Where(user => user.IsSubscribed 
                                   )
                    .Select(user =>
                        new object[]
                        {
                            user.EmailForNewsletter,
                            message
                        }).ToArray(),
                subject: subject);
        }

    }
}
