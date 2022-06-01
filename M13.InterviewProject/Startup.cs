using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace M13.InterviewProject
{
    using Repository;
    using Repository.Implementation;
    using Services;
    using Services.Implementation;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            AddApplicationServices(services);
            services.AddMvc(o =>
            {
                o.EnableEndpointRouting = false;
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebApi", Version = "v1"});
            });
        }

        private static void AddApplicationServices(IServiceCollection collection)
        {
            collection.AddScoped<IHttpClientFactory, HttpClientFactory>();
            collection.AddSingleton<IRulesRepository, RulesRepository>();
        }

        public static void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "WebApi v1"
                )
            );

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
