namespace CRMitServer.Core
{
    public class ConfirmingPurchaseHandler : IPurchaseHandler
    {
        private IEventContainer CurrentEventContainer { get; }

        public ConfirmingPurchaseHandler(IEventContainer eventContainer)
        {
            CurrentEventContainer = eventContainer;
        }

        public void HandleRequest(PurchaseRequest request)
        {
            var args = new ClientEventArgs(request.SenderClient);
            CurrentEventContainer.SendPurchaseMessage(args);
        }
    }
}
