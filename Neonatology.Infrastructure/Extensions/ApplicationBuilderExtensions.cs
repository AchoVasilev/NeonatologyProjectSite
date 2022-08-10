namespace Neonatology.Infrastructure.Extensions;

using System.Globalization;
using Common.Constants;
using Hangfire;
using Hangfire.Dashboard;
using Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using static Common.Constants.WebConstants.ApplicationBuilderExtensionsConstants;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder ConfigureLocalization(this IApplicationBuilder builder)
    {
        var supportedCultures = new[] { new CultureInfo("bg-BG") };
        builder.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("bg-BG"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        return builder;
    }

    public static IApplicationBuilder ConfigureApplication(this IApplicationBuilder builder)
        => builder.UseStatusCodePagesWithRedirects("/Home/Error/{0}")
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseCookiePolicy()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization();

    public static IApplicationBuilder ConfigureHangfireDashboard(this IApplicationBuilder builder)
        => builder.UseHangfireDashboard(
            "/hangfire",
            new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });
    

    public static IApplicationBuilder UseMvcWithAreas(this IApplicationBuilder builder)
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
    
    private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
        }
    }
}