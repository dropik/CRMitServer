using CRMitServer.Api;
using CRMitServer.Core;
using CRMitServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

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
            services.AddControllers();

            services.AddSingleton<IApplication, Application>();
            services.AddTransient<IPurchaseHandler, ConfirmingPurchaseHandler>((provider) =>
            {
                Func<Client, Task> purchaseAction = provider.GetService<IResponseSender>().SendToClientAsync;
                return new ConfirmingPurchaseHandler(purchaseAction);
            });
            services.AddSingleton<IResponseSender, EmailResponseSender>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton(Configuration.GetSection("PurchaseResponseSettings").Get<PurchaseResponseSettings>());
            services.AddSingleton(Configuration.GetSection("EmailClientSettings").Get<EmailClientSettings>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
