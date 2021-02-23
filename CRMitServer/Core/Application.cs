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

        public async Task HandlePurchaseRequestAsync(PurchaseRequest request)
        {
            var data =  await GetPurchaseDataAsync(request);
            await purchaseHandler.HandleAsync(data);
        }

        private async Task<PurchaseData> GetPurchaseDataAsync(PurchaseRequest request)
        {
            var getClientTask = database.GetClientByIdAsync(request.ClientId);
            var getItemTask = database.GetItemByIdAsync(request.ItemId);

            await Task.WhenAll(getClientTask, getItemTask);

            return new PurchaseData() {
                SenderClient = await getClientTask,
                Item = await getItemTask
            };
        }
    }
}
