namespace Infrastructure;

using System;
using System.Collections.Generic;
using CloudinaryDotNet;
using Data;
using Data.Models;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Administration;
using Services.AppointmentCauseService;
using Services.AppointmentService;
using Services.ChatService;
using Services.CityService;
using Services.DoctorService;
using Services.EmailSenderService;
using Services.FeedbackService;
using Services.FileService;
using Services.NotificationService;
using Services.OfferService;
using Services.PatientService;
using Services.PaymentService;
using Services.ProfileService;
using Services.RatingService;
using Services.SlotService;
using Services.SpecializationService;
using Services.UserService;
using ViewModels.Hubs;

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
                options.Password.RequiredUniqueChars = 0;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<NeonatologyDbContext>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<NeonatologyDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddMyControllers(this IServiceCollection services)
    {
        services.AddControllersWithViews(configure =>
        {
            configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });

        services.AddAntiforgery(options => { options.HeaderName = "X-CSRF-TOKEN"; });

        services.AddControllers();

        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services
            .AddTransient<IAppointmentService, AppointmentService>()
            .AddTransient<IPatientService, PatientService>()
            .AddTransient<IDoctorService, DoctorService>()
            .AddTransient<ICityService, CityService>()
            .AddTransient<IRatingService, RatingService>()
            .AddTransient<IFileService, FileService>()
            .AddTransient<ISpecializationService, SpecializationService>()
            .AddTransient<ISlotService, SlotService>()
            .AddTransient<IOfferService, OfferService>()
            .AddTransient<IChatService, ChatService>()
            .AddTransient<IUserService, UserService>()
            .AddTransient<IAppointmentCauseService, AppointmentCauseService>()
            .AddTransient<IPaymentService, PaymentService>()
            .AddTransient<INotificationService, NotificationService>()
            .AddTransient<IProfileService, ProfileService>()
            .AddTransient<IFeedbackService, FeedbackService>()
            .AddTransient<IGalleryService, GalleryService>()
            .AddTransient<IEmailSender, MailKitSender>()
            .AddTransient<ReCaptchaService>();

        return services;
    }

    public static IServiceCollection ConfigureMailkit(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailKitEmailSenderOptions>(options =>
        {
            options.HostAddress = configuration["SmtpSettings:Server"];
            options.HostPort = Convert.ToInt32(configuration["SmtpSettings:Port"]);
            options.HostUsername = configuration["SmtpSettings:Username"];
            options.HostPassword = configuration["SmtpSettings:Password"];
            options.SenderEmail = configuration["SmtpSettings:SenderEmail"];
            options.SenderName = configuration["SmtpSettings:SenderName"];
        });

        return services;
    }

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
    {
        services.AddHangfire(configuration =>
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString,
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));

        services.AddHangfireServer();

        return services;
    }

    public static IServiceCollection RegisterVoiceCallEntities(this IServiceCollection services)
    {
        services.AddSingleton<List<User>>();
        services.AddSingleton<List<UserCall>>();
        services.AddSingleton<List<CallOffer>>();

        return services;
    }

    public static IServiceCollection ConfigureCookies(this IServiceCollection services)
    {
        services
            .ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        return services;
    }
}