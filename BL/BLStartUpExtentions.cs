using BL.Helper;
using BL.Repository;
using BL.SawtoothClient;
using BL.SchemaEditBuilder;
using BL.service;
using GSchema;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public static class BLStartUpExtentions
    {
        public static IServiceCollection AddBLServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericBL<>), typeof(RepoBL<>));
            services.AddScoped<BlHelper>();
            //services.AddScoped(typeof(IAuthenTicationService), typeof(AuthenTicationService));
            services.AddScoped<EditBuilder>();
            services.AddScoped<GSgenerator>();
            services.AddScoped<getEnumList>();
            services.AddScoped<ClientKeyManager>();
            return services;
        }
    }
}
