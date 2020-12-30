using System.Threading.Tasks;
using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IDatabase
    {
        Task<Client> GetClientById(int id);
        Task<PurchaseItem> GetItemById(int id);
    }
}
