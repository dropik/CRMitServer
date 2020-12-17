namespace CRMitServer.Core
{
    public interface IPurchaseHandler
    {
        void Handle(PurchaseRequest request);
    }
}
