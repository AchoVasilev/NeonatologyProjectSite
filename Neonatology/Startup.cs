namespace Neonatology
{
    using System;

    using CloudinaryDotNet;

    using global::Data;
    using global::Data.Models;
    using global::Data.Seeding;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Services.AppointmentService;
    using Services.CityService;
    using Services.DateTimeParser;
    using Services.DoctorService;
    using Services.EmailSenderService;
    using Services.ImageService;
    using Services.PatientService;
    using Services.RatingService;
    using Services.SlotService;
    using Services.SpecializationService;

    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NeonatologyDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
            })
                 .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<NeonatologyDbContext>();

            services.Configure<IdentityOptions>(opts =>
            {
                opts.Lockout.AllowedForNewUsers = true;
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opts.Lockout.MaxFailedAccessAttempts = 3;
            });

            services.AddControllersWithViews(configure =>
            {
                configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddSingleton(Configuration);
            services.AddRazorPages();
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            services
                .AddTransient<IAppointmentService, AppointmentService>()
                .AddTransient<IPatientService, PatientService>()
                .AddTransient<IDateTimeParserService, DateTimeParserService>()
                .AddTransient<IDoctorService, DoctorService>()
                .AddTransient<ICityService, CityService>()
                .AddTransient<IRatingService, RatingService>()
                .AddTransient<IImageService, ImageService>()
                .AddTransient<ISpecializationService, SpecializationService>()
                .AddTransient<ISlotService, SlotService>();

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
            var cloudinaryAccount = new Account(cloud, apiKey, apiSecret);
            var cloudinary = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinary);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseRouting();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapPost("/appointment/generateSlots", async context =>
                //{
                //    var cancellationToken = context.RequestAborted;

                //    context.Response.ContentType = "text/plain";
                //    context.Response.Headers[HeaderNames.CacheControl] = "no-cache";

                //    await context.Response.StartAsync(cancellationToken);
                //});

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
