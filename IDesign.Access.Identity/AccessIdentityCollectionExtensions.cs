using IDesign.Access.Entities;
using IDesign.Access.Identity;
using IDesign.Access.Identity.JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace IDesign.Access
{
    public static class AccessIdentityCollectionExtensions
    {
        public static void AddAccessesIdentity(this IServiceCollection services)
        {
            services.AddTransient<IJwtProvider, JwtProvider>();
            services.Configure<PasswordHasherOptions>(o => o.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3);
            services.AddTransient<IPasswordHasher<User>, UserPasswordHasher>();
            services.AddTransient<IPasswordHasher, UserPasswordHasher>();

            services.AddTransient<IContextProvider, ContextProvider>();

            services.AddHttpContextAccessor();
        }
    }
}
