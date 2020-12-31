using NUnit.Framework;
using CRMitServer.Core;
using CRMitServer.Models;

namespace CRMitServer.UnitTests.Core
{
    [TestFixture]
    public class EventContainerTests
    {
        private EventContainer eventContainer;
        private Client client;

        const string CLIENT_NAME = "Ivan";

        [SetUp]
        public void SetUp()
        {
            eventContainer = new EventContainer();
            client = new Client()
            {
                Name = CLIENT_NAME
            };
        }

        [Test]
        public void TestSendPurchaseMessageInvokesPurchaseEvent()
        {
            eventContainer.Purchase += AssertEventCalled;
            eventContainer.SendPurchaseMessage(client);
            Assert.Fail("Event Purchase was not invoked.");
        }

        private void AssertEventCalled(Client client)
        {
            Assert.That(client.Name == CLIENT_NAME);
            Assert.Pass();
        }

        [Test]
        public void TestSendPurchaseDoesNotFailOnMissingHandlers()
        {
            try
            {
                eventContainer.SendPurchaseMessage(client);
                Assert.Pass();
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("NullReferenceException occured on trying to send a purchase message.");
            }
        }
    }
}
