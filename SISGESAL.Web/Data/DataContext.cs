using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data.Entities;

namespace SISGESAL.web.Data
{

    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //CREAMOS LA PROPIEDAD DE TIPO DBSET DE LAS TABLAS
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Almacen>? Almacens { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Court> Courts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.DNI).IsUnique();

            modelBuilder.Entity<Department>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Department>().HasIndex(x => x.CodDepHn).IsUnique();

            modelBuilder.Entity<Municipality>().HasIndex(x => x.CodMunHn).IsUnique();

            modelBuilder.Entity<Court>().HasIndex(x => x.Name).IsUnique();
        }
    }
}
