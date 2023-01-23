using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Infrastructure.EntityConfigurations
{
    public class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog", CatalogContext.DEFAULT_SCHEMA);
            builder.Property(t=>t.Id)
                .UseHiLo("catalog_hilo")
                .IsRequired();

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Price)
                .IsRequired(true);

            builder.Property(t => t.PictureFileName)
                .IsRequired(false);

            builder.Ignore(t => t.PictureUri);

            builder.HasOne(t => t.CatalogBrand)
                .WithMany()
                .HasForeignKey(t => t.CatalogBrandId);

            builder.HasOne(t => t.CatalogType)
              .WithMany()
              .HasForeignKey(t => t.CatalogTypeId);

        }
    }
}
