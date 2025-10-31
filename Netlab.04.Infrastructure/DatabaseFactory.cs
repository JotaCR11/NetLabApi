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
        IDatabase GetDatabaseNetlab1();
    }

    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly string _connectionString;
        private readonly string _connectionStringNetlab1;
        public DatabaseFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnectionVPNLocal");
            _connectionStringNetlab1 = configuration.GetConnectionString("DefaultConnectionNetlab1qa");

        }

        public IDatabase GetDatabase()
        {
            return new NPocoDatabase(_connectionString, new SqlServer2012DatabaseType(), Microsoft.Data.SqlClient.SqlClientFactory.Instance);
        }

        public IDatabase GetDatabaseNetlab1()
        {
            return new NPocoDatabase(_connectionStringNetlab1, new SqlServer2012DatabaseType(), Microsoft.Data.SqlClient.SqlClientFactory.Instance);
        }
    }
}
