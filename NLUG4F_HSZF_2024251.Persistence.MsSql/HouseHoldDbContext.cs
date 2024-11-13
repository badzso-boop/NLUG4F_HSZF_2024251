using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class HouseHoldDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Pantry> Pantry { get; set; }
        public DbSet<Fridge> Fridge { get; set; }

        public HouseHoldDbContext()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=householddb;Integrated Security=false;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fridge és Product közötti kapcsolat beállítása
            modelBuilder.Entity<Fridge>()
                        .HasMany(f => f.Products)
                        .WithOne(p => p.Fridge)
                        .HasForeignKey(p => p.FridgeId)
                        .IsRequired(false);

            // Pantry és Product közötti kapcsolat beállítása
            modelBuilder.Entity<Pantry>()
                        .HasMany(p => p.Products)
                        .WithOne(p => p.Pantry)
                        .HasForeignKey(p => p.PantryId)
                        .IsRequired(false);
        }
    }
}
