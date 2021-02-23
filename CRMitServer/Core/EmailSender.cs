using CRMitServer.Api;
using CRMitServer.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace CRMitServer.Core
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailClientSettings settings;

        public EmailSender(EmailClientSettings settings)
        {
            this.settings = settings;
        }

        public string Mailto { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }

        public async Task SendAsync()
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(settings.Email));
            message.To.Add(MailboxAddress.Parse(Mailto));
            message.Subject = EmailSubject;
            message.Body = new TextPart()
            {
                Text = EmailBody
            };

            using (var client = new SmtpClient())
            {
                client.Connect(settings.Server, settings.Port);
                client.Authenticate(settings.Email, settings.Password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
