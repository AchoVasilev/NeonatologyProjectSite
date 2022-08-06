namespace Infrastructure.Extensions;

using Hubs;
using Microsoft.AspNetCore.Builder;
using static Common.Constants.WebConstants.ApplicationBuilderExtensionsConstants;
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMvcWithWithAreas(this IApplicationBuilder builder)
        => builder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: AreasName,
                pattern: AreasPattern);

            endpoints.MapControllerRoute(
                name: DefaultName,
                pattern: DefaultPattern);

            endpoints.MapRazorPages();

            endpoints.MapHub<ChatHub>(ChatHubPattern);
            endpoints.MapHub<NotificationHub>(NotificationHubPattern);
            endpoints.MapHub<ConnectionHub>(ConnectionHubPattern);
        });
}