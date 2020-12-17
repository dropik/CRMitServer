using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IDatabase
    {
        Client GetClientById(int id);
        PurchaseItem GetItemById(int id);
    }
}