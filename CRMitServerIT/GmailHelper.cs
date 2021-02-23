using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CRMitServer.IT
{
    public class GmailHelper : IDisposable
    {
        private GmailService service;

        public static async Task<GmailHelper> CreateHelperAsync()
        {
            var helper = new GmailHelper();
            var builder = new ConfigurationBuilder().AddUserSecrets<GmailHelper>();
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

            helper.service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });

            return helper;
        }

        public async Task DeleteTestMessagesAsync()
        {
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

        public void Dispose()
        {
            service.Dispose();
        }

        public async Task<IList<Message>> GetMessagesByQueryAsync(string query)
        {
            var gmailRequest = service.Users.Messages.List("me");
            gmailRequest.Q = query;
            var messagesResponse = await gmailRequest.ExecuteAsync();
            var messages = messagesResponse.Messages;
            return messages;
        }
    }
}
