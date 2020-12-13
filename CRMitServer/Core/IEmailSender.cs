namespace CRMitServer.Core
{
    public interface IEmailSender
    {
        string Mailto { get; set; }
        string EmailBody { get; set; }
        string Object { get; set; }

        void Send();
    }
}
