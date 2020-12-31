using System;
using CRMitServer.Api;
using CRMitServer.Models;

namespace CRMitServer.Core
{
    public class EventContainer : IEventContainer
    {
        public event Action<Client> Purchase;

        public void SendPurchaseMessage(Client client)
        {
            Purchase?.Invoke(client);
        }
    }
}
