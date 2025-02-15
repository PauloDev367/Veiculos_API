using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeiculosApi.Models;

namespace VeiculosApi.Data.Configurations;

public class VehicleVariantConfig : IEntityTypeConfiguration<VehicleVariation>
{
    public void Configure(EntityTypeBuilder<VehicleVariation> builder)
    {
        builder.ToTable("vehicles_variations");
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

        builder.Property(x => x.Color)
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnType("FLOAT")
            .IsRequired();

    }
}
