using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAuthDotnetCoreSPA.Services;

[assembly: HostingStartup(typeof(TwitchAuthDotnetCoreSPA.TwitchHostingStartup))]
namespace TwitchAuthDotnetCoreSPA {
	public class TwitchHostingStartup : IHostingStartup {
		public void Configure(IWebHostBuilder builder) {
            builder.ConfigureServices(
                (context, services) => {
                    TwitchSettings twitchSettings = context.Configuration.GetSection("Twitch").Get<TwitchSettings>();
                    services.AddSingleton(twitchSettings);
                    services.AddSingleton<TwitchService>();
                });
		}
	}
}
