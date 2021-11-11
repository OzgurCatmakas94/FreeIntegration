using FreeIntegration.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using FreeIntegration.Services.EmailService;

namespace FreeIntegration
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                //Lockout
                options.Lockout.MaxFailedAccessAttempts = 3;//3 yanlýþ giriþten sonra kullanýcýyý kilitle
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//5 dk sonra tekrar login olabilsin
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Index";
                options.LogoutPath= "/Home/Index";
                //options.AccessDeniedPath= "/Home/Index";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name="FreeIntegration.Security.Cookie",

                };
            });

            services.AddScoped<IDbInitializer, DbInitializer>();
            //services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddSingleton<IEmailSender, EmailSender>();
            //services.Configure<EmailOptions>(Configuration);
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IDbInitializer dbInitializer)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRouting();
            dbInitializer.Initialize();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "areas",
                //    pattern: "{area=Contents}/{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
