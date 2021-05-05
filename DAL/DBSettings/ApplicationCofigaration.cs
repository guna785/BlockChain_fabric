using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL.DBSettings
{
    public class ApplicationCofigaration
    {
        public ApplicationCofigaration()
        {
            var configBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configBuilder.AddJsonFile(path, false);
            var root = configBuilder.Build();
            var conStrConfig = root.GetSection("DatabaseSettings:ConnectionString");
            var conDB = root.GetSection("DatabaseSettings:DatabaseName");
            connectionString = conStrConfig.Value;
            DB = conDB.Value;
        }
        public string connectionString { get; set; }
        public string DB { get; set; }
    }
}
