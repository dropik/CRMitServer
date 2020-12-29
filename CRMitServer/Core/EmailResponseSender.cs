using CRMitServer.Api;
using CRMitServer.Models;

namespace CRMitServer.Core
{
    public class EmailResponseSender : IResponseSender
    {
        private readonly IEmailSender emailSender;
        private readonly PurchaseResponseSettings settings;

        public EmailResponseSender(IEmailSender emailSender, PurchaseResponseSettings settings)
        {
            this.emailSender = emailSender;
            this.settings = settings;
        }

        public void SendToClient(Client client)
        {
            emailSender.EmailObject = settings.EmailObject;
            emailSender.EmailBody = settings.EmailBody;
            emailSender.Mailto = client.Email;
            emailSender.Send();
        }
    }
}
