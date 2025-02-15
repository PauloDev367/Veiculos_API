using System;
using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data.Configurations;
using VeiculosApi.Models;

namespace VeiculosApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleVariation> VehicleVariations { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CategoryConfig());
        modelBuilder.ApplyConfiguration(new PhotoConfig());
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new VehicleConfig());
        modelBuilder.ApplyConfiguration(new VehicleVariantConfig());
    }

}
