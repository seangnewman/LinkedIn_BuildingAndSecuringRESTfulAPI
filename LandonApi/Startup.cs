using AutoMapper;
using LandonApi.Filters;
using LandonApi.Infrastructure;
using LandonApi.Models;
using LandonApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;

namespace LandonApi
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

            services.Configure<HotelInfo>(Configuration.GetSection("Info"));


            // Any service with DbContext needs the AddScope 
            services.AddScoped<IRoomService, DefaultRoomService>();

            // Use an in-memory db for quick dev and testing
            // TODO : Swap out for real DB
            services.AddDbContext<HotelApiDbContext>(options => options.UseInMemoryDatabase("landondb"));

            services.AddMvc(options => { 
                                                                    options.Filters.Add<JsonExceptionFilter>();
                                                                    options.Filters.Add<RequireHttpsOnCloseAttribute>();
                                                                     options.Filters.Add<LinkRewritingFilter>();
      
                                                                })
                                                                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);



            services.AddRouting(Options => Options.LowercaseUrls = true);

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

            services.AddAutoMapper(options => options.AddProfile<MappingProfile>());

            services.AddCors(options =>
            {
                options.AddPolicy( "AllowMyApp",
                                                    policy => policy.AllowAnyOrigin());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUi3WithApiExplorer(options =>
                {
                    options.GeneratorSettings.DefaultPropertyNameHandling = NJsonSchema.PropertyNameHandling.CamelCase;
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("AllowMyApp");
            
            app.UseMvc();
        }
    }
}
