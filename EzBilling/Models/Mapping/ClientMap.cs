using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EzBilling.Models.Mapping
{
    public class ClientMap : EntityTypeConfiguration<Client>
    {
        public ClientMap()
        {
            // Primary Key
            this.HasKey(t => t.ClientId);

            // Properties
            this.Property(t => t.ClientId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

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
            this.ToTable("Client");
            this.Property(t => t.ClientId).HasColumnName("client_id");
            this.Property(t => t.Name).HasColumnName("name");
            this.Property(t => t.Address.Street).HasColumnName("address");
            this.Property(t => t.Address.City).HasColumnName("city");
            this.Property(t => t.Address.PostalCode).HasColumnName("postal_code");
        }
    }
}
