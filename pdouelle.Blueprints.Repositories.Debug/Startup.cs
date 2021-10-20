using System;
using System.Linq;
using AutoFixture;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace pdouelle.Blueprints.Repositories.Debug
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
            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("Database"));
            
            services.AddScoped<IRepository<WeatherForecast>, Repository<WeatherForecast, DatabaseContext>>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "pdouelle.Blueprints.Repositories.Debug", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "pdouelle.Blueprints.Repositories.Debug v1"));
            }

            app.UseHttpsRedirection();
            
            SeedDatabase(context);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void SeedDatabase(DatabaseContext context)
        {
            context.Database.EnsureCreated();
            
            if (!context.WeatherForecasts.Any())
            {
                var fixture = new Fixture();
                
                context.WeatherForecasts.Add(new WeatherForecast()
                {
                    Date = fixture.Create<DateTime>(),
                    Summary = fixture.Create<string>(),
                    TemperatureC = fixture.Create<int>(),
                    TemperatureF = fixture.Create<int>(),
                });

                context.SaveChanges();
            }
        }
    }
}