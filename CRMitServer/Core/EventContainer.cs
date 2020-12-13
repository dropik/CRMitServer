using System;

namespace CRMitServer.Core
{
    public class EventContainer : IEventContainer
    {
        public event EventHandler<ClientEventArgs> Purchase;

        public void SendPurchaseMessage(ClientEventArgs args)
        {
            Purchase?.Invoke(this, args);
        }
    }
}
