using NUnit.Framework;
using Moq;
using CRMitServer.Core;

namespace CRMitServer.UnitTests
{
    [TestFixture]
    public class ApplicationTests
    {
        [Test]
        public void TestPurchaseHandlerCalledOnPurchaseRequest()
        {
            const int ITEM_ID = 100;
            var purchaseHandler = new Mock<IPurchaseHandler>();
            var application = new Application()
            {
                CurrentPurchaseHandler = purchaseHandler.Object
            };
            var purchaseRequest = new PurchaseRequest()
            {
                ItemId = ITEM_ID
            };

            application.HandlePurchaseRequest(purchaseRequest);

            purchaseHandler
                .Verify(mock => mock.HandleRequest(
                    It.Is<PurchaseRequest>(req => req.ItemId == ITEM_ID)),
                    Times.Once);
        }

        [Test]
        public void TestPurchaseHandlingOnNullHandler()
        {
            var application = new Application();
            var purchaseRequest = new PurchaseRequest();

            try
            {
                application.HandlePurchaseRequest(purchaseRequest);
                Assert.Pass();
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Apparently Application attemted to call purchase handler, when it was not set.");
            }
        }
    }
}
