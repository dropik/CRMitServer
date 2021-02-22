using NUnit.Framework;
using CRMitServer.Core;
using CRMitServer.Models;
using System.Threading.Tasks;
using System;

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
        public async Task TestSendPurchaseMessageInvokesPurchaseEvent()
        {
            eventContainer.Purchase += AssertEventCalled;
            await eventContainer.SendPurchaseMessage(client);
            Assert.Fail("Event Purchase was not invoked.");
        }

        private Task AssertEventCalled(Client client)
        {
            Assert.That(client.Name == CLIENT_NAME);
            Assert.Pass();
            return Task.CompletedTask;
        }

        [Test]
        public async Task TestSendPurchaseDoesNotFailOnMissingHandlers()
        {
            try
            {
                await eventContainer.SendPurchaseMessage(client);
                Assert.Pass();
            }
            catch (NullReferenceException)
            {
                Assert.Fail("NullReferenceException occured on trying to send a purchase message.");
            }
        }
    }
}
