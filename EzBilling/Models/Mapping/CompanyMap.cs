using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EzBilling.Models.Mapping
{
    public class CompanyMap : EntityTypeConfiguration<Company>
    {
        public CompanyMap()
        {
            // Primary Key
            this.HasKey(t => t.CompanyID);

            // Properties
            this.Property(t => t.CompanyID)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.BankName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BankBIC)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.AccountNumber)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.BillerName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Phone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Address.City)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Address.PostalCode)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(5);

            this.Property(t => t.Address.Street)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Company");
            this.Property(t => t.CompanyID).HasColumnName("company_id");
            this.Property(t => t.Name).HasColumnName("name");
            this.Property(t => t.Address.Street).HasColumnName("address");
            this.Property(t => t.Address.City).HasColumnName("city");
            this.Property(t => t.Address.PostalCode).HasColumnName("postal_code");
            this.Property(t => t.BankName).HasColumnName("bank_name");
            this.Property(t => t.BankBIC).HasColumnName("bank_bic");
            this.Property(t => t.AccountNumber).HasColumnName("bank_account");
            this.Property(t => t.BillerName).HasColumnName("biller_name");
            this.Property(t => t.Email).HasColumnName("email");
            this.Property(t => t.Phone).HasColumnName("phone_number");
        }
    }
}
