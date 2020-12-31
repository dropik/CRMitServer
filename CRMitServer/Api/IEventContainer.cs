using System;
using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IEventContainer
    {
        event Action<Client> Purchase;
        void SendPurchaseMessage(Client client);
    }
}
