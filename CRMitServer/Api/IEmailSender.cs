using System.Threading.Tasks;

namespace CRMitServer.Api
{
    public interface IEmailSender
    {
        string Mailto { get; set; }
        string EmailBody { get; set; }
        string EmailSubject { get; set; }

        Task SendAsync();
    }
}
