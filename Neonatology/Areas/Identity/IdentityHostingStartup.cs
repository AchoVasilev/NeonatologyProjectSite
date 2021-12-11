using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Neonatology.Areas.Identity.IdentityHostingStartup))]
namespace Neonatology.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}