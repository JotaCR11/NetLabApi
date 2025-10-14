using Microsoft.Extensions.Configuration;
using NPoco;
using NPoco.DatabaseTypes;
using NPoco.SqlServer;
using NPocoDatabase = NPoco.Database;

namespace Netlab.Infrastructure.Database
{
    public interface IDatabaseFactory
    {
        IDatabase GetDatabase(); 
    }

    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly string _connectionString;
        public DatabaseFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnectionProd");
        }

        public IDatabase GetDatabase()
        {
            return new NPocoDatabase(_connectionString, new SqlServer2012DatabaseType(), Microsoft.Data.SqlClient.SqlClientFactory.Instance);
        }
    }
}
