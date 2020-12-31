using System.Threading.Tasks;
using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IDatabase
    {
        Task<Client> GetClientByIdAsync(int id);
        Task<PurchaseItem> GetItemByIdAsync(int id);
    }
}
