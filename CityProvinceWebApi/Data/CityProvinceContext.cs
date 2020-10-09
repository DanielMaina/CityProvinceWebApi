using CityProvinceWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityProvinceWebApi.Data
{
    public class CityProvinceContext : DbContext
    {
        public CityProvinceContext(DbContextOptions<CityProvinceContext> options)
            : base(options)
        {

        }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add a unique index to the combination 
            //of city name and provinceid
            modelBuilder.Entity<City>()
            .HasIndex(c => new { c.Name, c.ProvinceID })
            .IsUnique();

            //Add a unique index to the province code
            modelBuilder.Entity<Province>()
            .HasIndex(c => c.Code)
            .IsUnique();

            //Restrict Cascade Delete
            modelBuilder.Entity<Province>()
                .HasMany(p => p.Cities)
                .WithOne(d => d.Province)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
