using E_Commerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Persistence.Context.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .HasColumnType("VarChar")
            .HasMaxLength(256);

        builder.Property(p => p.Description)
            .HasColumnType("VarChar")
            .HasMaxLength(1024);

        builder.Property(p => p.PictureUrl)
            .HasColumnType("VarChar")
            .HasMaxLength(256);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(10,2)");

        builder.HasOne(p => p.ProductBrand)
            .WithMany()
            .HasForeignKey(p => p.BrandId);

        builder.HasOne(p => p.ProductType)
            .WithMany()
            .HasForeignKey(p => p.TypeId);
    }
}
