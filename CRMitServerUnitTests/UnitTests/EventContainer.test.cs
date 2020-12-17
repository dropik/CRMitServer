using NUnit.Framework;
using CRMitServer.Core;

namespace CRMitServer.UnitTests
{
    [TestFixture]
    public class EventContainerTests
    {
        private EventContainer eventContainer;
        private ClientEventArgs args;

        const string CLIENT_NAME = "Ivan";

        [SetUp]
        public void SetUp()
        {
            eventContainer = new EventContainer();
            var client = new Client()
            {
                Name = CLIENT_NAME
            };
            args = new ClientEventArgs(client);
        }

        [Test]
        public void TestSendPurchaseMessageInvokesPurchaseEvent()
        {
            eventContainer.Purchase += AssertEventCalled;
            eventContainer.SendPurchaseMessage(args);
            Assert.Fail("Event Purchase was not invoked.");
        }

        private void AssertEventCalled(object sender, ClientEventArgs args)
        {
            Assert.That(args.TargetClient.Name == CLIENT_NAME);
            Assert.Pass();
        }

        [Test]
        public void TestSendPurchaseDoesNotFailOnMissingHandlers()
        {
            try
            {
                eventContainer.SendPurchaseMessage(args);
                Assert.Pass();
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("NullReferenceException occured on trying to send a purchase message.");
            }
        }
    }
}
