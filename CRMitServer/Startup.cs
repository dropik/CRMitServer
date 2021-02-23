using CRMitServer.Api;
using CRMitServer.Core;
using CRMitServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRMitServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IApplication, Application>();
            services.AddTransient<IPurchaseHandler, ConfirmingPurchaseHandler>();
            services.AddSingleton<IEventContainer, EventContainer>();
            services.AddSingleton<IResponseSender, EmailResponseSender>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton(Configuration.GetSection("PurchaseResponseSettings").Get<PurchaseResponseSettings>());
            services.AddSingleton(Configuration.GetSection("EmailClientSettings").Get<EmailClientSettings>());

            var provider = services.BuildServiceProvider();
            var eventContainer = provider.GetService<IEventContainer>();
            var responseSender = provider.GetService<IResponseSender>();
            eventContainer.Purchase += responseSender.SendToClientAsync;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
