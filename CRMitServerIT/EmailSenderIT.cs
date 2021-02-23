using CRMitServer.Api;
using CRMitServer.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRMitServer.IT
{
    [TestFixture]
    public class EmailSenderIT
    {
        private WebApplicationFactory<Startup> factory;
        private GmailHelper gmailHelper;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            factory = new WebApplicationFactory<Startup>();

            gmailHelper = await GmailHelper.CreateHelperAsync();
            await gmailHelper.DeleteTestMessagesAsync();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            factory.Dispose();
            gmailHelper.Dispose();
        }

        [Test]
        public async Task TestEmailSent()
        {
            // Arrange
            var database = CreateMockDatabase();
            var expectedSubject = GenerateTestSubject();
            var client = CreateAppClient(database, expectedSubject);

            // Act
            var response = await ObtainResponseAsync(client);

            // Assert
            response.EnsureSuccessStatusCode();
            await AssertMessageSentAsync(expectedSubject);
        }

        private IDatabase CreateMockDatabase()
        {
            var mockDatabase = new Mock<IDatabase>();
            mockDatabase.Setup(m => m.GetClientByIdAsync(It.IsAny<int>())).ReturnsAsync(new Client()
            {
                Name = "Ivan",
                Email = "crmitserver.tests@gmail.com"
            });
            mockDatabase.Setup(m => m.GetItemByIdAsync(It.IsAny<int>())).ReturnsAsync(new PurchaseItem()
            {
                ItemId = 0
            });
            return mockDatabase.Object;
        }

        private string GenerateTestSubject()
        {
            var random = new Random();
            var testMessageNumber = random.Next().ToString();
            return "Test Message #" + testMessageNumber;
        }

        private HttpClient CreateAppClient(IDatabase database, string testSubject) => 
            factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(database);
                    services.AddSingleton(new PurchaseResponseSettings()
                    {
                        EmailSubject = testSubject,
                        EmailBody = "Thank you for the purchase!"
                    });
                });
            }).CreateClient();

        private async Task<HttpResponseMessage> ObtainResponseAsync(HttpClient client)
        {
            var request = new PurchaseRequest()
            {
                ClientId = 0,
                ItemId = 0
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            return await client.PostAsync("/api/purchase", jsonContent);
        }

        private async Task AssertMessageSentAsync(string expectedSubject)
        {
            var messages = await gmailHelper.GetMessagesByQueryAsync(expectedSubject);
            Assert.That(messages, Is.Not.Null);
            Assert.That(messages.Count, Is.EqualTo(1));
        }
    }
}
