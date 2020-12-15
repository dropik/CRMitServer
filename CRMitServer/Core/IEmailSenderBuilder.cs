namespace CRMitServer.Core
{
    public interface IEmailSenderBuilder
    {
        IEmailSender Make();
    }
}
