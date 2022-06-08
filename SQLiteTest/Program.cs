using System.Reflection;

namespace SQLiteTest
{
    using System;

    internal static class Program
    {
        public static void Main()
        {
            using (var context = LocalCachingDBContext.Create("."))
            {
                string assemblyName = Assembly.GetExecutingAssembly().FullName ?? "";

                context.LocalCacheItems.Add(new LocalCacheItem
                {
                    AssemblyQualifiedName = assemblyName,
                    CacheKey = "test key",
                    LastModifiedDate = DateTime.UtcNow,
                    Data = "Hello, world!"
                });

                context.SaveChanges();
            }
        }
    }
}