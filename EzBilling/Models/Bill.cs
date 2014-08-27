using System;
using System.Collections.Generic;

namespace EzBilling.Models
{
    public partial class Bill
    {
        public Bill()
        {
            this.Products = new List<Product>();
        }

        public long BillId { get; set; }
        public string CompanyId { get; set; }
        public long ClientId { get; set; }
        public string Reference { get; set; }
        public long DueDate { get; set; }
        public string AdditionalInformation { get; set; }
        public virtual Company Company { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
