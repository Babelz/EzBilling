using System;
using System.Collections.Generic;

namespace EzBilling.Models
{
    public class Product
    {
        public Product()
        {
            Bills = new List<Bill>();
        }

        public long ProductId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public long UnitPrice { get; set; }
        public string Unit { get; set; }
        public decimal VATPercent { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
