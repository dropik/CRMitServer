using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using CRMitServer.Api;
using CRMitServer.Controllers;
using CRMitServer.Exceptions;
using System.Threading.Tasks;

namespace CRMitServer.UnitTests.Controllers
{
    [TestFixture]
    public class PurchaseControllerTests
    {
        private Mock<IApplication> mockApp;
        private PurchaseController purchaseController;

        private const int CLIENT_ID = 12;
        private const int ITEM_ID = 100;

        [SetUp]
        public void SetUp()
        {
            mockApp = new Mock<IApplication>();
            purchaseController = new PurchaseController(mockApp.Object);
        }

        [Test]
        public async Task TestRequestHandledByApp()
        {
            _ = await ExecutePurchase();
            AssertApplicationRequestedForPurchase();
        }

        private async Task<IActionResult> ExecutePurchase()
        {
            return await purchaseController.Purchase(CLIENT_ID, ITEM_ID);
        }

        private void AssertApplicationRequestedForPurchase()
        {
            mockApp.Verify(
                mock => mock.HandlePurchaseRequestAsync(
                    It.Is<int>(clientId => clientId == CLIENT_ID),
                    It.Is<int>(itemId => itemId == ITEM_ID)),
                Times.Once);
        }

        [Test]
        public async Task TestOkResult()
        {
            var result = await ExecutePurchase();
            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public async Task TestBadRequestOnExceptionThrown()
        {
            SetupApplicationToThrowException();
            var result = await ExecutePurchase();
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        private void SetupApplicationToThrowException()
        {
            mockApp.Setup(mock => mock.HandlePurchaseRequestAsync(It.IsAny<int>(), It.IsAny<int>()))
                   .Throws<RequestException>();
        }
    }
}