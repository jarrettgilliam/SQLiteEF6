namespace SQLiteTest;

using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using DbContext = System.Data.Entity.DbContext;

public class LocalCachingDBContext : DbContext
{
    
        public const string LocalCacheDirectory = "LocalCache";
        public const string LocalCacheFileName = "LocalCaching.sdf";

        public LocalCachingDBContext(string connectionString)
            : base(new SqlConnection { ConnectionString = connectionString }, true)
        {
        }

        public System.Data.Entity.DbSet<LocalCacheItem> LocalCacheItems
        {
            get;
            set;
        }

        public static LocalCachingDBContext Create(string localStoragePath)
        {
            localStoragePath = Path.Combine(localStoragePath, LocalCacheDirectory);

            string databasePath = Path.Combine(localStoragePath, LocalCacheFileName);

            // Create folder structure if it does not already exist
            try
            {
                if (!Directory.Exists(localStoragePath))
                {
                    Directory.CreateDirectory(localStoragePath);
                }
            }
            catch
            {
            }

            var builder = new SqliteConnectionStringBuilder();
            builder.DataSource = databasePath;
            builder.Password = Environment.MachineName;

            string connectionString = builder.ToString();

            if (!File.Exists(databasePath))
            {
                CreateDB(connectionString);
            }

            LocalCachingDBContext context = new LocalCachingDBContext(connectionString);

            context.Database.Log = s => Debug.WriteLine(s);

            return context;
        }

        private static void CreateDB(string connectionString)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
            }
            catch
            {
            }
        }
}