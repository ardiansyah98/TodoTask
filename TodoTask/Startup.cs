using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TodoTask.Data;

namespace TodoTask
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
            // configure API behaviour options below to bypass validation on API Controller, 
            // so we can request todo class data without full required data
            // example : {
            //              "CompletePercentage" : 20
            //           }
            services.AddControllers().ConfigureApiBehaviorOptions(o => { o.SuppressModelStateInvalidFilter = true; }); ;

            services.AddDbContext<TodoTaskContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TodoTaskContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TodoTaskContext todoTaskContext)
        {
            //Migrate databse on startup with dependency injection
            todoTaskContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
