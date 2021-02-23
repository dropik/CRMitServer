using CRMitServer.Api;
using CRMitServer.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRMitServer.IT
{
    [TestFixture]
    public class EmailSenderIT
    {
        private WebApplicationFactory<Startup> factory;
        private GmailService service;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            factory = new WebApplicationFactory<Startup>();

            // creating gmail service
            var builder = new ConfigurationBuilder().AddUserSecrets<EmailSenderIT>();
            var config = builder.Build();
            var secrets = config.GetSection("GmailAPICredentials").Get<ClientSecrets>();

            var scopes = new string[] { GmailService.Scope.MailGoogleCom };
            var applicationName = "CRMitServerIT";
            var credPath = "CRMitServerIT/token";

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath)
            );

            service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });

            // cleaning up eventual tests
            var gmailRequest = service.Users.Messages.List("me");
            gmailRequest.Q = "Test Message";
            var messagesResponse = await gmailRequest.ExecuteAsync();
            var messages = messagesResponse.Messages;
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    var deleteRequest = service.Users.Messages.Delete("me", message.Id);
                    await deleteRequest.ExecuteAsync();
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            factory.Dispose();
        }

        [Test]
        public async Task TestEmailSent()
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

            var random = new Random();
            var testMessageNumber = random.Next().ToString();
            var expectedEmailObject = "Test Message #" + testMessageNumber;

            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(mockDatabase.Object);
                    services.AddSingleton(new PurchaseResponseSettings()
                    {
                        EmailSubject = expectedEmailObject,
                        EmailBody = "Thank you for the purchase!"
                    });
                });
            }).CreateClient();

            var request = new PurchaseRequest()
            {
                ClientId = 0,
                ItemId = 0
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/purchase", jsonContent);

            response.EnsureSuccessStatusCode();

            var gmailRequest = service.Users.Messages.List("me");
            gmailRequest.Q = expectedEmailObject;
            var messagesResponse = await gmailRequest.ExecuteAsync();
            var messages = messagesResponse.Messages;
            Assert.That(messages, Is.Not.Null);
            Assert.That(messages.Count, Is.EqualTo(1));
        }
    }
}
