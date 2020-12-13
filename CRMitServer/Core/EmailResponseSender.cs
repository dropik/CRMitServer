namespace CRMitServer.Core
{
    public class EmailResponseSender : IResponseSender
    {
        private readonly IEmailSender emailSender;
        private readonly string emailBody;
        private readonly string emailObject;

        public EmailResponseSender(IEmailSender emailSender, string emailBody, string emailObject)
        {
            this.emailSender = emailSender;
            this.emailBody = emailBody;
            this.emailObject = emailObject;
        }

        public void SendToClient(Client client)
        {
            emailSender.Mailto = client.Email;
            emailSender.EmailBody = emailBody;
            emailSender.Object = emailObject;
            emailSender.Send();
        }
    }
}
