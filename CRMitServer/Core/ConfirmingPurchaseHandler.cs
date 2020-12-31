using CRMitServer.Api;
using CRMitServer.Models;

namespace CRMitServer.Core
{
    public class ConfirmingPurchaseHandler : IPurchaseHandler
    {
        private readonly IEventContainer eventContainer;

        public ConfirmingPurchaseHandler(IEventContainer eventContainer)
        {
            this.eventContainer = eventContainer;
        }

        public void Handle(PurchaseRequest request)
        {
            eventContainer.SendPurchaseMessage(request.SenderClient);
        }
    }
}
