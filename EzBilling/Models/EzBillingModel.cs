using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EzBilling.Models.Mapping;

namespace EzBilling.Models
{
    public class EzBillingModel : DbContext
    {
        static EzBillingModel()
        {
            //Database.SetInitializer<EzBillingModel>(null);
        }

        public EzBillingModel()
            : base("Name=EzBillingModel")
        {
            
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BillMap());
            modelBuilder.Configurations.Add(new ClientMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new ProductMap());
        }
    }
}
