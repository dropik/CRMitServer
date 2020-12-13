using NUnit.Framework;
using Moq;
using CRMitServer.Core;

namespace CRMitServer.UnitTests
{
    [TestFixture]
    public class PurchaseResponderTests
    {
        [Test]
        public void TestResponseIsSent()
        {
            const string CLIENT_NAME = "Ivan";
            var eventContainer = new Mock<IEventContainer>();
            var sender = new Mock<IResponseSender>();
            var client = new Client() { Name = CLIENT_NAME };
            var clientArgs = new ClientEventArgs(client);
            var responder = new PurchaseResponder(eventContainer.Object, sender.Object);

            eventContainer.Raise(container => container.Purchase += null, clientArgs);

            sender
                .Verify(mock => mock.SendToClient(
                    It.Is<Client>(args => args.Name == CLIENT_NAME)),
                    Times.Once);
        }
    }
}
