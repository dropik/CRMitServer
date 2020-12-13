namespace CRMitServer.Core
{
    public class ConfirmingPurchaseHandler : IPurchaseHandler
    {
        private readonly IEventContainer eventContainer;

        public ConfirmingPurchaseHandler(IEventContainer eventContainer)
        {
            this.eventContainer = eventContainer;
        }

        public void HandleRequest(PurchaseRequest request)
        {
            var args = new ClientEventArgs(request.SenderClient);
            eventContainer.SendPurchaseMessage(args);
        }
    }
}
