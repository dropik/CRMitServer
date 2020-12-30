using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using CRMitServer.Api;
using CRMitServer.Core;
using CRMitServer.Models;

namespace CRMitServer.UnitTests.Core
{
    [TestFixture]
    public class ApplicationTests
    {
        private Mock<IDatabase> mockDb;
        private Mock<IPurchaseHandler> mockPurchaseHandler;
        private Application application;

        private const int CLIENT_ID = 12;
        private const int ITEM_ID = 100;
        private const string CLIENT_NAME = "Ivan";

        [SetUp]
        public void SetUp()
        {
            mockDb = new Mock<IDatabase>();
            mockPurchaseHandler = new Mock<IPurchaseHandler>();
            application = new Application(mockDb.Object, mockPurchaseHandler.Object);
        }

        [Test]
        public async Task TestClientResolvedOnPurchaseRequest()
        {
            await ExecutePurchaseRequest();
            AssertTriedToResolveClient();
        }

        private async Task ExecutePurchaseRequest()
        {
            await application.HandlePurchaseRequest(CLIENT_ID, ITEM_ID);
        }

        private void AssertTriedToResolveClient()
        {
            mockDb.Verify(
                m => m.GetClientById(It.Is<int>(id => id == CLIENT_ID)),
                Times.Once);
        }

        [Test]
        public async Task TestPurchaseItemResolvedOnPurchaseRequest()
        {
            await ExecutePurchaseRequest();
            AssertTriedToResolveItem();
        }

        private void AssertTriedToResolveItem()
        {
            mockDb.Verify(
                m => m.GetItemById(It.Is<int>(id => id == ITEM_ID)),
                Times.Once);
        }

        [Test]
        public async Task TestPurchaseRequestSendedToHandler()
        {
            SetupDbResults();
            await ExecutePurchaseRequest();
            AssertPurchaseHandlerCalledCorrectly();
        }

        private void SetupDbResults()
        {
            mockDb.Setup(m => m.GetClientById(It.IsAny<int>()))
                  .Returns(Task.FromResult(new Client() {Name = CLIENT_NAME }));
            mockDb.Setup(m => m.GetItemById(It.IsAny<int>()))
                  .Returns(Task.FromResult(new PurchaseItem() { ItemId = ITEM_ID }));
        }

        private void AssertPurchaseHandlerCalledCorrectly()
        {
            mockPurchaseHandler
                .Verify(m => m.Handle(It.Is<PurchaseRequest>(req =>
                            (req.SenderClient.Name == CLIENT_NAME) && (req.Item.ItemId == ITEM_ID))),
                        Times.Once);
        }
    }
}
