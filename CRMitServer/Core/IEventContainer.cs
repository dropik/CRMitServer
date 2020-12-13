using System;

namespace CRMitServer.Core
{
    public interface IEventContainer
    {
        event EventHandler<ClientEventArgs> Purchase;
        void SendPurchaseMessage(ClientEventArgs args);
    }
}
