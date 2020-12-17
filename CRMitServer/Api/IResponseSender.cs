using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IResponseSender
    {
        void SendToClient(Client client);
    }
}
