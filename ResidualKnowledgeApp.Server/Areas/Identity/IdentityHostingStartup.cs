using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResidualKnowledgeApp.Server.Areas.Identity.Data;
using ResidualKnowledgeApp.Server.Data;

[assembly: HostingStartup(typeof(ResidualKnowledgeApp.Server.Areas.Identity.IdentityHostingStartup))]
namespace ResidualKnowledgeApp.Server.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ResidualKnowledgeAppServerContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ResidualKnowledgeAppServerContextConnection")));

                services.AddDefaultIdentity<ResidualKnowledgeAppServerUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ResidualKnowledgeAppServerContext>();
            });
        }
    }
}
