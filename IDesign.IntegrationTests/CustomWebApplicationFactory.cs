using IDesign.Access;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IDesign.IntegrationTests
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DesignDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a new DbContext registration with the desired connection string
                ((IServiceCollection)services).AddDbContext<DesignDbContext>(options =>
                    options.UseSqlite("Data Source=test.db"));

                // Build the service provider to ensure the DbContext is available
                var sp = ((IServiceCollection)services).BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DesignDbContext>();
                    db.Database.EnsureDeleted(); // delete existing database if any
                }
            });
        }
    }
}
