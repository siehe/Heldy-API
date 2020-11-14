using System.Threading.Tasks;
using Heldy.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Heldy.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendStudentRegistrationEmailAsync(string email, string password)
        {
            var message =
                $"Dear Student,\r\n\r\nWelcome to the distance learning system Heldy. \r\nThis is your login and password for entering the system.\r\nFeel free to contact us in case of problems.\r\n\r\nLogin: {email}\r\nPassword: {password}\r\n\r\nSincerely,\r\n\r\nViktor Kauk, Ph.D., assistant professor of Software Engineering\r\nKharkiv National University of Radio Electronics\r\n\r\n61166 Kharkov, Ukraine, Lenina 14, 330\r\nwww.nure.ua\r\nphone / fax: +38057-7021385 :: mob.+38067-5720965\r\nviktor.kauk@nure.ua \r\nSkype: victor.kauk\r\n";
            await SendEmailAsync(email, message);
        }

        private async Task SendEmailAsync(string email, string message)
        {
            var emailMessage = CreateEmailMessage(email, message);
            await SendEmailMessageAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(string email, string message)
        {
            var emailMessage = new MimeMessage();
            var senderName = configuration["Email:SenderName"];
            var senderEmail = configuration["Email:SenderEmail"];

            emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Registration";
            emailMessage.Body = new TextPart("Plain")
            {
                Text = message
            };

            return emailMessage;
        }

        private async Task SendEmailMessageAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                var host = configuration["Email:Host"];
                var hostPort = int.Parse(configuration["Email:HostPort"]);
                await client.ConnectAsync(host, hostPort, false);
                var senderEmail = configuration["Email:SenderEmail"];
                var emailPassword = configuration["Email:Password"];
                await client.AuthenticateAsync(senderEmail, emailPassword);
                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }
    }
}