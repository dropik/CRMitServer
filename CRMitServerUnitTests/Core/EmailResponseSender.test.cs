using NUnit.Framework;
using Moq;
using CRMitServer.Api;
using CRMitServer.Core;
using CRMitServer.Models;
using System.Threading.Tasks;

namespace CRMitServer.UnitTests.Core
{
    [TestFixture]
    public class EmailResponseSenderTests
    {
        private Mock<IEmailSender> emailSender;
        private PurchaseResponseSettings settings;
        private Client client;
        private EmailResponseSender responseSender;

        private const string CLIENT_EMAIL = "test@example.com";
        private const string TEST_OBJECT = "Purchase confirmed";
        private const string TEST_BODY = "Thank you for purchase!";

        [SetUp]
        public void SetUp()
        {
            SetupEmailSender();
            SetupSettings();
            SetupClient();
            SetupEmailResponseSender();
        }

        private void SetupEmailSender()
        {
            emailSender = new Mock<IEmailSender>();
        }

        private void SetupSettings()
        {
            settings = new PurchaseResponseSettings()
            {
                EmailObject = TEST_OBJECT,
                EmailBody = TEST_BODY
            };
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
            responseSender = new EmailResponseSender(emailSender.Object, settings);
        }

        private async Task Act()
        {
            await responseSender.SendToClientAsync(client);
        }

        [Test]
        public async Task TestEmailObjectIsSet()
        {
            await Act();
            emailSender.VerifySet(
                m => m.EmailObject = It.Is<string>(obj => obj == TEST_OBJECT),
                Times.Once
            );
        }

        [Test]
        public async Task TestEmailBodyIsSet()
        {
            await Act();
            emailSender.VerifySet(
                m => m.EmailBody = It.Is<string>(body => body == TEST_BODY),
                Times.Once
            );
        }

        [Test]
        public async Task TestMailtoIsSet()
        {
            await Act();
            emailSender.VerifySet(
                m => m.Mailto = It.Is<string>(email => email == CLIENT_EMAIL),
                Times.Once
            );
        }

        [Test]
        public async Task TestEmailSenderIsCalled()
        {
            await Act();
            emailSender.Verify(m => m.SendAsync(), Times.Once);
        }
    }
}
