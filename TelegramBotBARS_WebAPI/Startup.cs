using Microsoft.EntityFrameworkCore;
using TelegramBotBARS_WebAPI.Entities;
using TelegramBotBARS_WebAPI.Services;

namespace TelegramBotBARS_WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "swagger";
                });
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddNewtonsoftJson();

            services.AddSwaggerGen();

            services.AddControllers();

            services.AddDbContext<TGBot_BARS_DbContext>(options
                => options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL")));

            services.AddScoped<DbDataProvider>();
        }
    }
}
