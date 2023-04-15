using System;
using Microsoft.EntityFrameworkCore;

namespace testAppIrkit
{
    internal class AppDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=root;database=irkitdb;",
                new MySqlServerVersion(new Version(8, 0, 11)));
        }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }


        // Список сущностей таблицы
        public DbSet<Employee> Employees { get; set; }

    }
}
