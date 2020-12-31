using System.Threading.Tasks;

namespace CRMitServer.Api
{
    public interface IApplication
    {
        Task HandlePurchaseRequestAsync(int clientId, int itemId);
    }
}