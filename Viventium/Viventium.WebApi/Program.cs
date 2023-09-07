using Microsoft.EntityFrameworkCore;
using Viventium.Data;

namespace Viventium.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ViventiumContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            _createDbIfNotExists(app);

            app.Run();
        }

        private static void _createDbIfNotExists(WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                try
                {
                    ViventiumContext context = services.GetRequiredService<ViventiumContext>();

                    //recreate the database every time the app is started
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}