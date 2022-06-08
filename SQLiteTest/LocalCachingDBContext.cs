namespace SQLiteTest
{
    using System;
    using System.Data.SQLite;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.Data.Sqlite;
    using DbContext = System.Data.Entity.DbContext;

    public class LocalCachingDBContext : DbContext
    {
        public const string LocalCacheDirectory = "LocalCache";
        public const string LocalCacheFileName = "LocalCaching.sdf";

        public LocalCachingDBContext(string connectionString)
            : base(new SQLiteConnection() { ConnectionString = connectionString}, true)
        {
        }

        public System.Data.Entity.DbSet<LocalCacheItem> LocalCacheItems { get; set; }

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
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }

            string encryptionKey = Environment.MachineName;

            string connectionString = $"Data Source={databasePath};Password={encryptionKey}";

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
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }
    }
}