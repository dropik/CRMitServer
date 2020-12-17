namespace CRMitServer.Core
{
    public interface IApplication
    {
        void HandlePurchaseRequest(int clientId, int itemId);
    }
}