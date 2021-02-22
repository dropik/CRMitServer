using System;
using System.Threading.Tasks;
using CRMitServer.Api;
using CRMitServer.Models;

namespace CRMitServer.Core
{
    public class EventContainer : IEventContainer
    {
        public event Func<Client, Task> Purchase;

        public async Task SendPurchaseMessage(Client client)
        {
            if (Purchase != null)
            {
                await Purchase.Invoke(client);
            }
        }
    }
}
