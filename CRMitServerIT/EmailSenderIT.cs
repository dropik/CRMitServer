using CRMitServer.Api;
using CRMitServer.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRMitServer.IT
{
    [TestFixture]
    public class EmailSenderIT
    {
        internal class OnlyEmailSenderStartup : Startup
        {
            public OnlyEmailSenderStartup(IConfiguration configuration) : base(configuration) { }

            protected override void RegisterDatabase(IServiceCollection services)
            {
                services.AddSingleton(mockDatabase.Object);
            }
        }

        internal class CRMitServerApplicationFactory : WebApplicationFactory<Startup>
        {
            //protected override IWebHostBuilder CreateWebHostBuilder()
            //{
            //    var builder = new WebHostBuilder();
            //    builder.UseStartup<OnlyEmailSenderStartup>();
            //    return builder;
            //}
        }

        private CRMitServerApplicationFactory factory;
        private static Mock<IDatabase> mockDatabase;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            factory = new CRMitServerApplicationFactory();
        }

        [SetUp]
        public void SetUp()
        {
            mockDatabase = new Mock<IDatabase>();
        }

        [Test]
        public async Task TestEmailSent()
        {
            var client = factory.CreateClient();
            var request = new PurchaseRequest()
            {
                ClientId = 0,
                ItemId = 0
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/purchase", jsonContent);

            response.EnsureSuccessStatusCode();
            string[] scopes = { GmailService.Scope.MailGoogleCom };
            string applicationName = "CRMitServerIT";
            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "CRMitServerIT/token";
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }

            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });

            var gmailRequest = service.Users.Messages.List("me");
            gmailRequest.Q = "Test Message";
            var messagesResponse = await gmailRequest.ExecuteAsync();
            var messages = messagesResponse.Messages;
            Assert.That(messages.Count, Is.EqualTo(1));
        }
    }
}
