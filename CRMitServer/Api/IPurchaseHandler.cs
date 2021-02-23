using CRMitServer.Models;
using System.Threading.Tasks;

namespace CRMitServer.Api
{
    public interface IPurchaseHandler
    {
        Task HandleAsync(PurchaseData request);
    }
}
