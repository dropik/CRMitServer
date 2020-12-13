namespace CRMitServer.Core
{
    public class Application
    {
        public IPurchaseHandler CurrentPurchaseHandler { get; set; }

        public void HandlePurchaseRequest(PurchaseRequest request)
        {
            CurrentPurchaseHandler?.HandleRequest(request);
        }
    }
}
