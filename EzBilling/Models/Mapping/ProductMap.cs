using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EzBilling.Models.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductId);

            // Properties
            this.Property(t => t.ProductId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Unit)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Product");
            this.Property(t => t.ProductId).HasColumnName("product_id");
            this.Property(t => t.Name).HasColumnName("name");
            this.Property(t => t.Quantity).HasColumnName("quantity");
            this.Property(t => t.UnitPrice).HasColumnName("unit_price");
            this.Property(t => t.Unit).HasColumnName("unit");
            this.Property(t => t.VATPercent).HasColumnName("vat");
        }
    }
}
