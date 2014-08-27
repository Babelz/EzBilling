using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EzBilling.Models.Mapping
{
    public class BillMap : EntityTypeConfiguration<Bill>
    {
        public BillMap()
        {
            // Primary Key
            this.HasKey(t => t.BillId);

            // Properties
            this.Property(t => t.BillId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CompanyId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Reference)
                .HasMaxLength(100);

            this.Property(t => t.AdditionalInformation)
                .HasMaxLength(2147483647);

            // Table & Column Mappings
            this.ToTable("Bill");
            this.Property(t => t.BillId).HasColumnName("bill_id");
            this.Property(t => t.CompanyId).HasColumnName("company");
            this.Property(t => t.ClientId).HasColumnName("client");
            this.Property(t => t.Reference).HasColumnName("reference");
            this.Property(t => t.DueDate).HasColumnName("due_date");
            this.Property(t => t.AdditionalInformation).HasColumnName("comments");

            // Relationships
            this.HasMany(t => t.Products)
                .WithMany(t => t.Bills)
                .Map(m =>
                {
                    m.ToTable("BillItems");
                    m.MapLeftKey("bill_id");
                    m.MapRightKey("product_id");
                });

            this.HasRequired(t => t.Company)
                .WithMany(t => t.Bills)
                .HasForeignKey(d => d.CompanyId);
            this.HasRequired(t => t.Client)
                .WithMany(t => t.Bills)
                .HasForeignKey(d => d.ClientId);

        }
    }
}