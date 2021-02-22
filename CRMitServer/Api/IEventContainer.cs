using System;
using System.Threading.Tasks;
using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IEventContainer
    {
        event Func<Client, Task> Purchase;
        Task SendPurchaseMessage(Client client);
    }
}
