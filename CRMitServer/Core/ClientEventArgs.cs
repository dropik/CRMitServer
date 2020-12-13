using System;

namespace CRMitServer.Core
{
    public class ClientEventArgs : EventArgs
    {
        public Client TargetClient { get; private set; }

        public ClientEventArgs(Client client)
        {
            TargetClient = client;
        }
    }
}
