using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Models;

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

        public DbSet<Department> Departments { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Court> Courts { get; set; }

        public DbSet<Depot> Depots { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<KindofArticle> KindofArticles { get; set; }
        public DbSet<TradeMark> TradeMarks { get; set; }

        public DbSet<KindofPeople> KindofPeoples { get; set; }
        public DbSet<Gender> Genders { get; set; }

        public DbSet<KindofArticle> Articles { get; set; }

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

            modelBuilder.Entity<Depot>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<Supplier>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<KindofArticle>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<TradeMark>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<Article>().HasIndex(x => x.Name).IsUnique();
        }

        public DbSet<SISGESAL.web.Data.Entities.Depot> Supplier { get; set; } = default!;
        public DbSet<SISGESAL.web.Models.DepotViewModel> DepotViewModel { get; set; } = default!;
        public DbSet<SISGESAL.web.Data.Entities.Article> Article { get; set; } = default!;
    }
}