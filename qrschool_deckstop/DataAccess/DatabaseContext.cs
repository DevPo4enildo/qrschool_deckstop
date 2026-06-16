using Dapper;
using Npgsql;
using System.Data;

namespace qrschool_deckstop.DataAccess
{
    public static class DatabaseContext
    {
        static DatabaseContext()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        private static string _connectionString = "Host=ep-fragrant-tree-agdqtjqx-pooler.c-2.eu-central-1.aws.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_iX7oUAuj1sRC; SSL Mode=VerifyFull; Channel Binding=Require;";

        public static void Initialize(string connectionString)
        {
       
        }

        public static IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
