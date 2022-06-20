using FiltersExample.Filters;

namespace FiltersExample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddScoped<ActionFilterExample>(); // Filtro de controlador o de acción
            services.AddScoped<MetadaActionFilter>(); // Filtro de controlador o de acción

            services.AddControllers();
            /*
             * Para aplicar el filtro de forma global.
            builder.Services.AddControllers(options => options.Filters.Add(new MetadaActionFilter())); 

            */


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("Dev", conf =>
                {
                    conf.AllowAnyHeader()
                    .AllowAnyMethod().AllowAnyOrigin();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("Dev");
            app.UseAuthorization();

            app.UseEndpoints(config =>
            {
                config.MapControllers();
            });
        }
    }
}
