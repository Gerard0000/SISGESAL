﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Models;
using System.Reflection.Metadata;

namespace SISGESAL.web.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User>(options)
    {
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

        public DbSet<Article> Articles { get; set; }

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

            //modelBuilder.Entity<User>().HasOne(x => x.Depotuser}).WithOne(e => e.User).HasForeignKey<Depot>("UserId");

            //bueno
            //modelBuilder.Entity<User>().HasOne(e => e.Depot).WithOne(e => e.User).HasForeignKey<Depot>(e => e.UserId).IsRequired(false);
            //modelBuilder.Entity<User>().HasOne(e => e.Depot).WithOne(e => e.User).HasForeignKey<Depot>(e => e.UserId).IsRequired(false);

            //EJEMPLO DE LA PAGINA
            //modelBuilder.Entity<Blog>().HasOne(e => e.Header).WithOne(e => e.Blog).HasForeignKey<BlogHeader>(e => e.BlogId).IsRequired(false);
        }
    }
}