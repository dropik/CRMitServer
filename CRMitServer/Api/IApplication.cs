namespace CRMitServer.Api
{
    public interface IApplication
    {
        void HandlePurchaseRequest(int clientId, int itemId);
    }
}