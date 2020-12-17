using CRMitServer.Api;
using CRMitServer.Models;

namespace CRMitServer.Core
{
    public class ResponseEmailBuilder : IEmailSenderBuilder
    {
        private readonly IEmailSenderFactory emailFactory;
        private readonly ApplicationSettings settings;

        public ResponseEmailBuilder(IEmailSenderFactory emailFactory, ApplicationSettings settings)
        {
            this.emailFactory = emailFactory;
            this.settings = settings;
        }

        public IEmailSender Make()
        {
            var sender = emailFactory.Create();
            sender.EmailBody = settings.PurchaseResponseEmailBody;
            sender.EmailObject = settings.PurchaseResponseEmailObject;
            return sender;
        }
    }
}
