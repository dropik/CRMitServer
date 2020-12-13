using System;
using NUnit.Framework;
using CRMitServer.Core;

namespace CRMitServer.UnitTests
{
    [TestFixture]
    public class EventContainerTests
    {
        const string CLIENT_NAME = "Ivan";

        [Test]
        public void TestSendPurchaseMessageInvokesPurchaseEvent()
        {
            var eventContainer = new EventContainer();
            eventContainer.Purchase += AssertEventCalled;
            var client = new Client()
            {
                Name = CLIENT_NAME
            };
            var args = new ClientEventArgs(client);

            eventContainer.SendPurchaseMessage(args);
            Assert.Fail("Event Purchase was not invoked.");
        }

        private void AssertEventCalled(object sender, ClientEventArgs args)
        {
            Assert.That(args.TargetClient.Name == CLIENT_NAME);
            Assert.Pass();
        }
    }
}
