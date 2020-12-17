using CRMitServer.Api;
using CRMitServer.Models;

namespace CRMitServer.Core
{
    public class EmailResponseSender : IResponseSender
    {
        private readonly IEmailSenderBuilder emailBuilder;

        public EmailResponseSender(IEmailSenderBuilder emailBuilder)
        {
            this.emailBuilder = emailBuilder;
        }

        public void SendToClient(Client client)
        {
            var emailSender = emailBuilder.Make();
            emailSender.Mailto = client.Email;
            emailSender.Send();
        }
    }
}
