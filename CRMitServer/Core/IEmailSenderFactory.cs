namespace CRMitServer.Core
{
    public interface IEmailSenderFactory
    {
        IEmailSender Create();
    }
}
