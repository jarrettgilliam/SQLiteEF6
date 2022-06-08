namespace SQLiteTest2
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.SQLite;
    using System.Linq;
    using SQLite.CodeFirst;

    internal class Program
    {
        static void Main()
        {
            using (var db = new MyContext(@"Data Source=Test.sqlite"))
            {
                var person = new Person
                {
                    Id = db.Persons.Max(p => p.Id + 1),
                    Name = "Jarrett"
                };

                db.Persons.Add(person);
                db.SaveChanges();
            }
        }
    }

    class Person
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
    }

    class MyContext : DbContext
    {
        public MyContext(string connectionString)
            : base(new SQLiteConnection { ConnectionString = connectionString }, true)
        {
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<MyContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}