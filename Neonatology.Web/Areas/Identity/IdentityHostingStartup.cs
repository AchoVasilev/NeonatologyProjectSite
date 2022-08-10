using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Neonatology.Web.Areas.Identity.IdentityHostingStartup))]
namespace Neonatology.Web.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => {
        });
    }
}