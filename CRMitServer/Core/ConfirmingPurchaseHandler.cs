using CRMitServer.Api;
using CRMitServer.Models;
using System;
using System.Threading.Tasks;

namespace CRMitServer.Core
{
    public class ConfirmingPurchaseHandler : IPurchaseHandler
    {
        private readonly Func<Client, Task> purchaseAction;

        public ConfirmingPurchaseHandler(Func<Client, Task> purchaseAction)
        {
            this.purchaseAction = purchaseAction;
        }

        public async Task HandleAsync(PurchaseData request)
        {
            if (purchaseAction != null)
            {
                await purchaseAction.Invoke(request.SenderClient);
            }
        }
    }
}
