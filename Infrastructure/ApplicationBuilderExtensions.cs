namespace Infrastructure;

using Hubs;
using Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMvcWithWithAreas(this IApplicationBuilder builder)
        => builder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapRazorPages();

            endpoints.MapHub<ChatHub>("/chatHub");
            endpoints.MapHub<NotificationHub>("/notificationHub");
            endpoints.MapHub<ConnectionHub>("/connectionHub");
        });
}