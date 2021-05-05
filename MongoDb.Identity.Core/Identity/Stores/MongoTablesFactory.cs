using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.Identity.Core.Identity.Stores
{
    public class MongoTablesFactory
    {
        private readonly string _connectionString;

        public const string TABLE_USERS = "Users";
        public const string TABLE_ROLES = "Roles";

        public MongoTablesFactory(IConfiguration configuration)
        {
            _connectionString = configuration["MongoDbSettings:ConnectionString"];
        }

        public IMongoCollection<T> GetCollection<T>(string tableName)
        {
            MongoClient client = new MongoClient(_connectionString);
            var db = client.GetDatabase("IdentityDb");
            return db.GetCollection<T>(tableName);
        }
    }
}
