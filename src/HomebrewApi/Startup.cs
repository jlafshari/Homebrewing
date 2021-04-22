using AutoMapper;
using HomebrewApi.AutoMapper;
using HomebrewApi.Models;
using HomebrewApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace HomebrewApi
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
            services.Configure<HomebrewingDatabaseSettings>(Configuration.GetSection(nameof(HomebrewingDatabaseSettings)));
            services.AddSingleton<IHomebrewingDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<HomebrewingDatabaseSettings>>().Value);

            services.AddSingleton<HomebrewingDbService>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DefaultProfile());
            });
            services.AddSingleton(mapperConfig.CreateMapper());
            
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomebrewApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomebrewApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
