using DatingApp.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // (DI container - every time we create something that we want to be consumed by another part of our application, we have to add it here
        // as a service to be avalaible to inject this service somewhere else in our application).
        // AddSingleton => not good if you make multiple requests simultaneously because there is only one instance of the repository.
        // AddTransient => create an instance of the repo each time a request is done. Good for lightweight
        // AddScoped => create one instance for each HTTP request but use the same instance of repo in the same request
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlite
            (Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddCors();
            // We will inject the IAuthRepository and use the logic and implementation of the AuthRepository
            services.AddScoped<IAuthRepository, AuthRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // When a request call our API, it goes throught a pipeline.
        // Everything added here is middleware (software used to interact with our request)
        // all the app. down here are middleware.
        // Order in of the middlewares in the pipeline is important
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            // Adding Cors to allowed source to acces to API's data
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
