using NUnit.Framework;
using Moq;
using CRMitServer.Core;

namespace CRMitServer.UnitTests
{
    [TestFixture]
    public class ResponseEmailBuilderTests
    {
        private Mock<IEmailSenderFactory> emailFactory;
        private Mock<IEmailSender> emailSender;
        private ApplicationSettings settings;
        private ResponseEmailBuilder builder;

        private const string EMAIL_BODY = "Thank you for purchase!";
        private const string EMAIL_OBJECT = "Purchase notification";
        private const string MOCK_SENDER = "Mock Sender";

        [SetUp]
        public void SetUp()
        {
            SetupEmailSender();
            SetupEmailFactory();
            SetupSettings();
            SetupBuilder();
        }

        private void SetupEmailSender()
        {
            emailSender = new Mock<IEmailSender>();
            emailSender.Setup(mock => mock.Equals(It.Is<string>(other => other == MOCK_SENDER)))
                       .Returns(true);
        }

        private void SetupEmailFactory()
        {
            emailFactory = new Mock<IEmailSenderFactory>();
            emailFactory.Setup(mock => mock.Create())
                        .Returns(emailSender.Object);
        }

        private void SetupSettings()
        {
            settings = new ApplicationSettings()
            {
                PurchaseResponseEmailBody = EMAIL_BODY,
                PurchaseResponseEmailObject = EMAIL_OBJECT
            };
        }

        private void SetupBuilder()
        {
            builder = new ResponseEmailBuilder(emailFactory.Object, settings);
        }

        [Test]
        public void TestBuilderUsesFactory()
        {
            builder.Make();
            emailFactory.Verify(mock => mock.Create(), Times.Once);
        }

        [Test]
        public void TestEmailBodyAssignedFromSettings()
        {
            builder.Make();
            emailSender.VerifySet(
                mock => mock.EmailBody = It.Is<string>(body => body == EMAIL_BODY));
        }

        [Test]
        public void TestEmailObjectAssignedFromSettings()
        {
            builder.Make();
            emailSender.VerifySet(
                mock => mock.EmailObject = It.Is<string>(obj => obj == EMAIL_OBJECT));
        }

        [Test]
        public void TestSenderFromFactoryIsReturnde()
        {
            var sender = builder.Make();
            Assert.That(sender.Equals(MOCK_SENDER));
        }
    }
}
