using NUnit.Framework;
using Moq;
using CRMitServer.Core;

namespace CRMitServer.UnitTests
{
    [TestFixture]
    public class EmailResponseSenderTests
    {
        private Mock<IEmailSender> emailSender;
        private EmailResponseSender responseSender;
        private Client client;
        private ApplicationSettings settings;

        private const string CLIENT_EMAIL = "test@example.com";
        private const string EMAIL_BODY = "Thank you for the purchase!";
        private const string EMAIL_OBJECT = "Purchase notification";

        [SetUp]
        public void SetUp()
        {
            emailSender = new Mock<IEmailSender>();
            settings = new ApplicationSettings()
            {
                PurchaseResponseEmailBody = EMAIL_BODY,
                PurchaseResponseEmailObject = EMAIL_OBJECT
            };
            client = new Client()
            {
                Email = CLIENT_EMAIL
            };
            responseSender = new EmailResponseSender(emailSender.Object, settings);
        }

        [Test]
        public void TestMailtoIsSet()
        {
            responseSender.SendToClient(client);
            emailSender
                .VerifySet(mock =>
                    mock.Mailto = It.Is<string>(email =>
                        email == CLIENT_EMAIL),
                    Times.Once);
        }

        [Test]
        public void TestResponseBodyIsSet()
        {
            responseSender.SendToClient(client);
            emailSender
                .VerifySet(mock =>
                    mock.EmailBody = It.Is<string>(body =>
                        body == EMAIL_BODY),
                    Times.Once);
        }

        [Test]
        public void TestObjectIsSet()
        {
            responseSender.SendToClient(client);
            emailSender
                .VerifySet(mock =>
                    mock.Object = It.Is<string>(obj =>
                        obj == EMAIL_OBJECT),
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
