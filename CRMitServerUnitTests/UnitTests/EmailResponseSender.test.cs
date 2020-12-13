using NUnit.Framework;
using Moq;
using CRMitServer.Core;

namespace CRMitServer.UnitTests
{
    [TestFixture]
    public class EmailResponseSenderTests
    {
        private Mock<IEmailSender> emailSender;
        private Mock<IEmailSenderFactory> emailSenderFactory;
        private EmailResponseSender responseSender;
        private Client client;

        private const string CLIENT_EMAIL = "test@example.com";

        [SetUp]
        public void SetUp()
        {
            emailSender = new Mock<IEmailSender>();
            emailSenderFactory = new Mock<IEmailSenderFactory>();
            emailSenderFactory.Setup(mock => mock.Create()).Returns(emailSender.Object);
            client = new Client()
            {
                Email = CLIENT_EMAIL
            };
        }

        [Test]
        public void TestFactoryWasCalled()
        {
            responseSender = new EmailResponseSender(emailSenderFactory.Object);
            responseSender.SendToClient(client);
            emailSenderFactory.Verify(mock => mock.Create(), Times.Once);
        }

        [Test]
        public void TestMailtoIsSet()
        {
            responseSender = new EmailResponseSender(emailSenderFactory.Object);
            responseSender.SendToClient(client);
            emailSender
                .VerifySet(mock =>
                    mock.Mailto = It.Is<string>(email =>
                        email == CLIENT_EMAIL),
                    Times.Once);
        }

        [Test]
        public void TestEmailSenderIsCalled()
        {
            responseSender = new EmailResponseSender(emailSenderFactory.Object);
            responseSender.SendToClient(client);
            emailSender.Verify(mock => mock.Send(), Times.Once);
        }
    }
}
