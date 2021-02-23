using NUnit.Framework;
using Moq;
using CRMitServer.Api;
using CRMitServer.Core;
using CRMitServer.Models;
using System.Threading.Tasks;
using System;

namespace CRMitServer.UnitTests.Core
{
    [TestFixture]
    public class ConfirmingPurchaseHandlerTests
    {
        private Client client;
        private Mock<Func<Client, Task>> mockPurchaseAction;
        private PurchaseData request;
        private ConfirmingPurchaseHandler purchaseHandler;

        private const string CLIENT_NAME = "Ivan";

        [SetUp]
        public void SetUp()
        {
            client = new Client()
            {
                Name = CLIENT_NAME
            };
            mockPurchaseAction = new Mock<Func<Client, Task>>();
            request = new PurchaseData()
            {
                SenderClient = client
            };
            purchaseHandler = new ConfirmingPurchaseHandler(mockPurchaseAction.Object);
        }

        [Test]
        public async Task TestHandleIfNoPurchaseActionProvided()
        {
            purchaseHandler = new ConfirmingPurchaseHandler(null);
            try
            {
                await purchaseHandler.HandleAsync(request);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task TestPurchaseEventIsInvoked()
        {
            await purchaseHandler.HandleAsync(request);

            mockPurchaseAction.Verify(
                m => m.Invoke(
                    It.Is<Client>(client => client.Name == CLIENT_NAME)
                ),
                Times.Once
            );
        }
    }
}
