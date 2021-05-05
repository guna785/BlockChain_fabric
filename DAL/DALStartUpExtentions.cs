using DAL.DALrepo;
using DAL.DALService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public static class DALStartUpExtentions
    {
        public static IServiceCollection AddDALServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericService<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
