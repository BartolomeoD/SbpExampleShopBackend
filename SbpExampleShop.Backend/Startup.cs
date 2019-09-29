using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SbpExampleShop.Backend.Abstractions;
using SbpExampleShop.Backend.Abstractions.Repositories;
using SbpExampleShop.Backend.Integration;
using SbpExampleShop.Backend.Logic;
using SbpExampleShop.Backend.Repositories;
using SbpExampleShop.Backend.Utils;

namespace SbpExampleShop.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration
            ;
        
        private const string AllowAllCors = "AllowAllCors";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=3a98940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<PaymentService>();
            services.AddHttpClient<IAkbarsSbpIntegration, AkbarsSbpIntegration>();
            services.AddTransient<IQrEncoder, QrEncoder>();
            services.AddSingleton<PaymentStatusService>();
            services.Configure<AkbarsSbpIntegrationOptions>(_configuration.GetSection("AkbarsSbpIntegrationOptions"));
            services.AddMvc();
            services.AddCors(options =>
                options.AddPolicy(AllowAllCors,
                    builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(AllowAllCors);
            app.UseMvc();
        }
    }
}