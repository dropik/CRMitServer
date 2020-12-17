using NUnit.Framework;
using Moq;
using CRMitServer.Core;

namespace CRMitServer.UnitTests
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
        public void TestClientResolvedOnPurchaseRequest()
        {
            ExecutePurchaseRequest();
            AssertTriedToResolveClient();
        }

        private void ExecutePurchaseRequest()
        {
            application.HandlePurchaseRequest(CLIENT_ID, ITEM_ID);
        }

        private void AssertTriedToResolveClient()
        {
            mockDb.Verify(
                mock => mock.GetClientById(It.Is<int>(id => id == CLIENT_ID)),
                Times.Once);
        }

        [Test]
        public void TestPurchaseItemResolvedOnPurchaseRequest()
        {
            ExecutePurchaseRequest();
            AssertTriedToResolveItem();
        }

        private void AssertTriedToResolveItem()
        {
            mockDb.Verify(
                mock => mock.GetItemById(It.Is<int>(id => id == ITEM_ID)),
                Times.Once);
        }

        [Test]
        public void TestPurchaseRequestSendedToHandler()
        {
            SetupDbResults();
            ExecutePurchaseRequest();
            AssertPurchaseHandlerCalledCorrectly();
        }

        private void SetupDbResults()
        {
            mockDb.Setup(mock => mock.GetClientById(It.IsAny<int>()))
                  .Returns(new Client() { Name = CLIENT_NAME });
            mockDb.Setup(mock => mock.GetItemById(It.IsAny<int>()))
                  .Returns(new PurchaseItem() { ItemId = ITEM_ID });
        }

        private void AssertPurchaseHandlerCalledCorrectly()
        {
            mockPurchaseHandler
                .Verify(mock => mock.Handle(It.Is<PurchaseRequest>(req =>
                            (req.SenderClient.Name == CLIENT_NAME) && (req.Item.ItemId == ITEM_ID))),
                        Times.Once);
        }
    }
}
