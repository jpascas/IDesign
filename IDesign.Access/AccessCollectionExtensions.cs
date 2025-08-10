using IDesign.Access.Entities;
using IDesign.Access.Identity;
using IDesign.Access.Identity.JWT;
using IDesign.Access.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace IDesign.Access
{
    public static class AccessCollectionExtensions
    {
        public static void AddAccesses(this IServiceCollection services)
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IJwtProvider, JwtProvider>();
            services.Configure<PasswordHasherOptions>(o => o.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3);
            services.AddTransient<IPasswordHasher<User>, UserPasswordHasher>();
            services.AddTransient<IPasswordHasher, UserPasswordHasher>();

            services.AddTransient<IContextProvider, ContextProvider>();

            services.AddHttpContextAccessor();
        }

        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DesignDbContext>();

            var pendingMigrations = context.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
