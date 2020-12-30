using System.Threading.Tasks;

namespace CRMitServer.Api
{
    public interface IApplication
    {
        Task HandlePurchaseRequest(int clientId, int itemId);
    }
}