using System;
using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IEventContainer
    {
        event EventHandler<ClientEventArgs> Purchase;
        void SendPurchaseMessage(ClientEventArgs args);
    }
}
