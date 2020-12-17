namespace CRMitServer.Core
{
    public class Application
    {
        private readonly IDatabase database;
        private readonly IPurchaseHandler purchaseHandler;

        public Application(IDatabase database, IPurchaseHandler purchaseHandler)
        {
            this.database = database;
            this.purchaseHandler = purchaseHandler;
        }

        public void HandlePurchaseRequest(int clientId, int itemId)
        {
            var request = ConstructPurchaseRequest(clientId, itemId);
            purchaseHandler.Handle(request);
        }

        private PurchaseRequest ConstructPurchaseRequest(int clientId, int itemId)
        {
            return new PurchaseRequest() {
                SenderClient = database.GetClientById(clientId),
                Item = database.GetItemById(itemId)
            };
        }
    }
}
