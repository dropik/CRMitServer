using CRMitServer.Models;

namespace CRMitServer.Api
{
    public interface IPurchaseHandler
    {
        void Handle(PurchaseRequest request);
    }
}
