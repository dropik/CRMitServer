using System.Threading.Tasks;
using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IResponseSender
    {
        Task SendToClientAsync(Client client);
    }
}
