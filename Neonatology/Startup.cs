namespace Neonatology
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using CloudinaryDotNet;

    using Common;

    using Data;
    using Data.Models;
    using Data.Seeding;

    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Neonatology.Areas.Administration.Services;
    using Hubs;

    using Services;
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

    using SignalRCoreWebRTC.Models;

    using Stripe;

    using ViewModels.GoogleRecaptcha;
    using ViewModels.Stripe;

    public class Startup
    {
        public Startup(IConfiguration configuration)
            => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
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

            services.AddDbContext<NeonatologyDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews(configure =>
            {
                configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

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

            services.AddSingleton(this.Configuration);
            services.AddRazorPages();
            services.AddControllers();
            services.AddSignalR();
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<List<User>>();
            services.AddSingleton<List<UserCall>>();
            services.AddSingleton<List<CallOffer>>();

            services
                .AddTransient<IAppointmentService, AppointmentService>()
                .AddTransient<IPatientService, PatientService>()
                .AddTransient<IDoctorService, DoctorService>()
                .AddTransient<ICityService, CityService>()
                .AddTransient<IRatingService, RatingService>()
                .AddTransient<IFileService, Services.FileService.FileService>()
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
                .AddTransient<ReCaptchaService>();

            //Configure ReCAPTCHA
            services.Configure<RecaptchaSetting>(this.Configuration.GetSection("GoogleRecaptchaV3"));

            //Configure Stripe
            services.Configure<StripeSettings>(this.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = this.Configuration["Stripe:SecretKey"];

            //Configure SMTP MailKit
            services.AddTransient<IEmailSender, MailKitSender>();
            services.Configure<MailKitEmailSenderOptions>(options =>
            {
                options.HostAddress = this.Configuration["SmtpSettings:Server"];
                options.HostPort = Convert.ToInt32(this.Configuration["SmtpSettings:Port"]);
                options.HostUsername = this.Configuration["SmtpSettings:Username"];
                options.HostPassword = this.Configuration["SmtpSettings:Password"];
                options.SenderEmail = this.Configuration["SmtpSettings:SenderEmail"];
                options.SenderName = this.Configuration["SmtpSettings:SenderName"];
            });

            //Configure Cloudinary
            var cloud = this.Configuration["Cloudinary:CloudifyName"];
            var apiKey = this.Configuration["Cloudinary:ApiKey"];
            var apiSecret = this.Configuration["Cloudinary:ApiSecret"];
            var cloudinaryAccount = new CloudinaryDotNet.Account(cloud, apiKey, apiSecret);
            var cloudinary = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinary);

            //Configure Hangfire
            services.AddHangfire(configuration =>
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(this.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[] { new CultureInfo("bg-BG") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("bg-BG"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.PrepareDatabase()
                .GetAwaiter()
                .GetResult();

            if (env.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard(
                "/hangfire",
                new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });

            app.UseEndpoints(endpoints =>
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

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();

                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }
    }
}