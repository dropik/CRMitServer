namespace CRMitServer.Core
{
    public interface IDatabase
    {
        Client GetClientById(int id);
        PurchaseItem GetItemById(int id);
    }
}