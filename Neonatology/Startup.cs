namespace Neonatology;

using Data.Seeding;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Extensions;
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
        var connectionString = this.Configuration.GetConnectionString("LinuxConnection");
        services.AddIdentity()
            .AddDatabase(connectionString)
            .AddDatabaseDeveloperPageExceptionFilter()
            .AddMyControllers()
            .RegisterServices()
            .ConfigureCookies()
            .AddSingleton(this.Configuration)
            .ConfigureMailkit(this.Configuration)
            .AddCloudinary(this.Configuration)
            .RegisterHangfire(connectionString)
            .RegisterVoiceCallEntities()
            .AddRazorPages();

        services.AddSignalR();
        services.AddAutoMapper(typeof(Startup));

        //Configure ReCAPTCHA
        services.Configure<RecaptchaSetting>(this.Configuration.GetSection("GoogleRecaptchaV3"));

        //Configure Stripe
        services.Configure<StripeSettings>(this.Configuration.GetSection("Stripe"));
        StripeConfiguration.ApiKey = this.Configuration["Stripe:SecretKey"];
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ConfigureLocalization()
            .PrepareDatabase()
            .GetAwaiter()
            .GetResult();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage()
                .UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.ConfigureApplication()
            .ConfigureHangfireDashboard()
            .UseMvcWithAreas();
    }
}