using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace FreeIntegration.Api
{
    public class Startup
    {
        private readonly Container container = new Container();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IntegrateSimpleInjector(services);//Initialize container.
            
            services.AddSwaggerGen();
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddSimpleInjector(container, options =>
                       {
                           options.AddAspNetCore().AddControllerActivation();

                       });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            UseSwaggerInMvc(app);
            

            app.UseSimpleInjector(container);
            container.Verify();

        }

        private void UseSwaggerInMvc(IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(ui =>
            {
                ui.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeIntegration.Api");
                ui.RoutePrefix = string.Empty;
            });
        }
        private void RegisterServicesIoc()
        {
           // container.Register<ILoginService, LoginService>();
        }
        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            RegisterServicesIoc();
        }

    }
}
