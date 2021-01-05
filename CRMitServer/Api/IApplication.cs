using System.Threading.Tasks;
using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IApplication
    {
        Task HandlePurchaseRequestAsync(PurchaseRequest request);
    }
}