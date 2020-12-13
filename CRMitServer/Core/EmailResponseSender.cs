namespace CRMitServer.Core
{
    public class EmailResponseSender : IResponseSender
    {
        private readonly IEmailSenderFactory emailSenderFactory;

        public EmailResponseSender(IEmailSenderFactory emailSenderFactory)
        {
            this.emailSenderFactory = emailSenderFactory;
        }

        public void SendToClient(Client client)
        {
            var emailSender = emailSenderFactory.Create();
            emailSender.Mailto = client.Email;
            emailSender.Send();
        }
    }
}
