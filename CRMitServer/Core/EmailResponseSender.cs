namespace CRMitServer.Core
{
    public class EmailResponseSender : IResponseSender
    {
        private readonly IEmailSender emailSender;
        private readonly ApplicationSettings settings;

        public EmailResponseSender(IEmailSender emailSender, ApplicationSettings settings)
        {
            this.emailSender = emailSender;
            this.settings = settings;
        }

        public void SendToClient(Client client)
        {
            emailSender.Mailto = client.Email;
            emailSender.EmailBody = settings.PurchaseResponseEmailBody;
            emailSender.Object = settings.PurchaseResponseEmailObject;
            emailSender.Send();
        }
    }
}
