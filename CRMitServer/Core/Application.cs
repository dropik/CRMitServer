using System.Threading.Tasks;
using CRMitServer.Api;
using CRMitServer.Models;

namespace CRMitServer.Core
{
    public class Application : IApplication
    {
        private readonly IDatabase database;
        private readonly IPurchaseHandler purchaseHandler;

        public Application(IDatabase database, IPurchaseHandler purchaseHandler)
        {
            this.database = database;
            this.purchaseHandler = purchaseHandler;
        }

        public async Task HandlePurchaseRequestAsync(int clientId, int itemId)
        {
            var request =  await ConstructPurchaseRequestAsync(clientId, itemId);
            purchaseHandler.Handle(request);
        }

        private async Task<PurchaseRequest> ConstructPurchaseRequestAsync(int clientId, int itemId)
        {
            var getClientTask = database.GetClientByIdAsync(clientId);
            var getItemTask = database.GetItemByIdAsync(itemId);

            await Task.WhenAll(getClientTask, getItemTask);

            return new PurchaseRequest() {
                SenderClient = await getClientTask,
                Item = await getItemTask
            };
        }
    }
}
