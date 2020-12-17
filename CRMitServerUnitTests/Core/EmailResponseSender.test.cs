using NUnit.Framework;
using Moq;
using CRMitServer.Api;
using CRMitServer.Core;
using CRMitServer.Models;

namespace CRMitServer.UnitTests.Core
{
    [TestFixture]
    public class EmailResponseSenderTests
    {
        private Mock<IEmailSender> emailSender;
        private Mock<IEmailSenderBuilder> emailBuilder;
        private EmailResponseSender responseSender;
        private Client client;

        private const string CLIENT_EMAIL = "test@example.com";

        [SetUp]
        public void SetUp()
        {
            SetupEmailSender();
            SetupEmailBuilder();
            SetupClient();
            SetupEmailResponseSender();
        }

        private void SetupEmailSender()
        {
            emailSender = new Mock<IEmailSender>();
        }

        private void SetupEmailBuilder()
        {
            emailBuilder = new Mock<IEmailSenderBuilder>();
            emailBuilder.Setup(mock => mock.Make())
                        .Returns(emailSender.Object);
        }

        private void SetupClient()
        {
            client = new Client()
            {
                Email = CLIENT_EMAIL
            };
        }

        private void SetupEmailResponseSender()
        {
            responseSender = new EmailResponseSender(emailBuilder.Object);
        }

        [Test]
        public void TestBuilderWasCalled()
        {
            responseSender.SendToClient(client);
            emailBuilder.Verify(mock => mock.Make(), Times.Once);
        }

        [Test]
        public void TestMailtoIsSet()
        {
            responseSender.SendToClient(client);
            emailSender.VerifySet(
                mock => mock.Mailto = It.Is<string>(email => email == CLIENT_EMAIL),
                Times.Once);
        }

        [Test]
        public void TestEmailSenderIsCalled()
        {
            responseSender.SendToClient(client);
            emailSender.Verify(mock => mock.Send(), Times.Once);
        }
    }
}
