﻿using NUnit.Framework;
using Moq;
using CRMitServer.Api;
using CRMitServer.Core;
using CRMitServer.Models;
using System.Threading.Tasks;

namespace CRMitServer.UnitTests.Core
{
    [TestFixture]
    public class ConfirmingPurchaseHandlerTests
    {
        private Client client;
        private Mock<IEventContainer> mockEventContainer;
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
            mockEventContainer = new Mock<IEventContainer>();
            request = new PurchaseData()
            {
                SenderClient = client
            };
            purchaseHandler = new ConfirmingPurchaseHandler(mockEventContainer.Object);
        }

        [Test]
        public async Task TestPurchaseEventIsInvoked()
        {
            await purchaseHandler.HandleAsync(request);

            mockEventContainer.Verify(
                m => m.SendPurchaseMessage(
                    It.Is<Client>(client => client.Name == CLIENT_NAME)
                ),
                Times.Once
            );
        }
    }
}
