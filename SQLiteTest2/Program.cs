namespace SQLiteTest2
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using SQLite.CodeFirst;

    internal class Program
    {
        static void Main()
        {
            using (var db = new MyContext())
            {
                var person = new Person { Id = 1, Name = "John" };
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
        public MyContext() : base("MyContext")
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