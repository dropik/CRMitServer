using CRMitServer.Api;
using CRMitServer.Models;
using System.Threading.Tasks;

namespace CRMitServer.Core
{
    public class ConfirmingPurchaseHandler : IPurchaseHandler
    {
        private readonly IEventContainer eventContainer;

        public ConfirmingPurchaseHandler(IEventContainer eventContainer)
        {
            this.eventContainer = eventContainer;
        }

        public async Task HandleAsync(PurchaseData request)
        {
            await eventContainer.SendPurchaseMessage(request.SenderClient);
        }
    }
}
