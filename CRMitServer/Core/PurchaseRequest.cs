namespace CRMitServer.Core
{
    public class PurchaseRequest
    {
        public int ItemId { get; set; }
        public Client SenderClient { get; set; }
    }
}
