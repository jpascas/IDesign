using IDesign.Access;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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
        public static void AddManagers(this IServiceCollection services)
        {            
            services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<ICityManager, CityManager>();     

            services.AddSingleton(TimeProvider.System);
        }
    }
}
