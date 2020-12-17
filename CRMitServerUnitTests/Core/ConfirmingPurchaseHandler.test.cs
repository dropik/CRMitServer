using NUnit.Framework;
using Moq;
using CRMitServer.Api;
using CRMitServer.Core;
using CRMitServer.Models;

namespace CRMitServer.UnitTests.Core
{
    [TestFixture]
    public class ConfirmingPurchaseHandlerTests
    {
        [Test]
        public void TestPurchaseEventIsInvoked()
        {
            const string CLIENT_NAME = "Ivan";
            var client = new Client()
            {
                Name = CLIENT_NAME
            };
            var eventContainer = new Mock<IEventContainer>();
            var purchaseHandler = new ConfirmingPurchaseHandler(eventContainer.Object);
            var request = new PurchaseRequest()
            {
                SenderClient = client
            };

            purchaseHandler.Handle(request);

            eventContainer
                .Verify(mock => mock.SendPurchaseMessage(
                    It.Is<ClientEventArgs>(args => args.TargetClient.Name == CLIENT_NAME)),
                    Times.Once);
        }
    }
}
