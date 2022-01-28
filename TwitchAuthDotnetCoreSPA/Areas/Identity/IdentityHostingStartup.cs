using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitchAuthDotnetCoreSPA.Areas.Identity.Data;

[assembly: HostingStartup(typeof(TwitchAuthDotnetCoreSPA.Areas.Identity.IdentityHostingStartup))]
namespace TwitchAuthDotnetCoreSPA.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<TwitchAuthDotnetCoreSPAIdentityDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("TwitchAuthDotnetCoreSPAIdentityDbContextConnection")));

                //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //    .AddEntityFrameworkStores<TwitchAuthSPAIdentityDbContext>();
            });
        }
    }
}