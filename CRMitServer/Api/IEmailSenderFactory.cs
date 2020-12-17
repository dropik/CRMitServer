namespace CRMitServer.Api
{
    public interface IEmailSenderFactory
    {
        IEmailSender Create();
    }
}
