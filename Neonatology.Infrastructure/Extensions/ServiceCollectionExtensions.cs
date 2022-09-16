namespace Neonatology.Infrastructure.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet;
using Data;
using Data.Models;
using Hangfire;
using Hangfire.SqlServer;
using Hubs.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Common;
using Services.EmailSenderService;
using static Common.Constants.WebConstants.ServiceCollectionExtensionsConstants;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = IdentityPasswordUniqueChars;
                options.Lockout.MaxFailedAccessAttempts = MaxFailedAccessAttempts;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(DefaultLockoutTimeSpan);
            })
            .AddEntityFrameworkStores<NeonatologyDbContext>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        => services.AddDbContext<NeonatologyDbContext>(options =>
            options.UseSqlServer(connectionString));

    public static IServiceCollection AddMyControllers(this IServiceCollection services)
    {
        services.AddControllersWithViews(configure =>
        {
            configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });

        services.AddAntiforgery(options => { options.HeaderName = XCSRFHeader; });

        services.AddControllers();

        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        var transientRegistration = typeof(ITransientService);
        var scopedRegistration = typeof(IScopedService);
        var singletonRegistration = typeof(ISingletonService);

        var types = transientRegistration
            .Assembly
            .GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(s => new
            {
                Service = s.GetInterface($"I{s.Name}"),
                Implementation = s
            })
            .Where(x => x.Service != null);

        foreach (var type in types)
        {
            if (transientRegistration.IsAssignableFrom(type.Service))
            {
                services.AddTransient(type.Service, type.Implementation);
            }
            else if (singletonRegistration.IsAssignableFrom(type.Service))
            {
                services.AddSingleton(type.Service, type.Implementation);
            }
            else if (scopedRegistration.IsAssignableFrom(type.Service))
            {
                services.AddScoped(type.Service, type.Implementation);
            }
        }

        return services;
    }

    public static IServiceCollection ConfigureMailkit(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<MailKitEmailSenderOptions>(options =>
        {
            options.HostAddress = configuration["SmtpSettings:Server"];
            options.HostPort = Convert.ToInt32(configuration["SmtpSettings:Port"]);
            options.HostUsername = configuration["SmtpSettings:Username"];
            options.HostPassword = configuration["SmtpSettings:Password"];
            options.SenderEmail = configuration["SmtpSettings:SenderEmail"];
            options.SenderName = configuration["SmtpSettings:SenderName"];
        });

    public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        var cloud = configuration["Cloudinary:CloudifyName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];
        var cloudinaryAccount = new Account(cloud, apiKey, apiSecret);
        var cloudinary = new Cloudinary(cloudinaryAccount);
        services.AddSingleton(cloudinary);

        return services;
    }

    public static IServiceCollection RegisterHangfire(this IServiceCollection services, string connectionString)
        => services.AddHangfire(configuration =>
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString,
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(HangFireTimeSpan),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(HangFireTimeSpan),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            DisableGlobalLocks = true
                        }))
            .AddHangfireServer();

    public static IServiceCollection RegisterVoiceCallEntities(this IServiceCollection services)
        => services
            .AddSingleton<List<User>>()
            .AddSingleton<List<UserCall>>()
            .AddSingleton<List<CallOffer>>();

    public static IServiceCollection ConfigureCookies(this IServiceCollection services)
        => services
            .ConfigureApplicationCookie(options =>
            {
                options.LoginPath = LoginPath;
                options.LogoutPath = LogoutPath;
                options.AccessDeniedPath = AccessDeniedPath;
            })
            .Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
}