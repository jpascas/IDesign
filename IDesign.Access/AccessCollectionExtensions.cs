using Microsoft.Extensions.DependencyInjection;


namespace IDesign.Access
{
    public static class AccessCollectionExtensions
    {
        public static void AddAccesses(this IServiceCollection services)
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
        }
    }
}
