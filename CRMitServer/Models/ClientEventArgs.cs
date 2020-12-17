using System;

namespace CRMitServer.Models
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
