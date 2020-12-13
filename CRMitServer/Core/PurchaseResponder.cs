namespace CRMitServer.Core
{
    public class PurchaseResponder
    {
        private IResponseSender Sender { get; }

        public PurchaseResponder(IEventContainer eventContainer, IResponseSender sender)
        {
            Sender = sender;
            eventContainer.Purchase += OnPurchase;
        }

        private void OnPurchase(object sender, ClientEventArgs args)
        {
            Sender.SendToClient(args.TargetClient);
        }
    }
}
