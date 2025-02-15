using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeiculosApi.Models;

namespace VeiculosApi.Data.Configurations;

public class VehicleConfig : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("TEXT")
            .IsRequired();

        builder.Property(x => x.Year)
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(x => x.Color)
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(x => x.FuelType)
            .HasColumnType("VARCHAR")
            .IsRequired();
            
        builder.Property(x => x.Price)
            .HasColumnType("FLOAT")
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .IsRequired();

        builder.HasMany(x => x.Variations)
            .WithOne(v => v.Vehicle)
            .HasForeignKey(v => v.VehicleId);

        builder.HasMany(x => x.Photos)
            .WithOne(p => p.Vehicle)
            .HasForeignKey(p => p.VehicleId);
    }
}
