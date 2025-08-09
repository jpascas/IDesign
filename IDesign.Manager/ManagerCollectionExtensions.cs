using IDesign.Access;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IDesign.Manager
{
    public static class ManagerCollectionExtensions
    {
        public static void AddManagersAndAcceses(this IServiceCollection services)
        {
            services.AddAccesses();
            services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<ICityManager, CityManager>();     

            services.AddSingleton(TimeProvider.System);
        }
    }
}
