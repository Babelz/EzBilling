using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzBilling.Models
{
    public class Product
    {
        #region Vars
        public long ProductId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public long UnitPrice { get; set; }
        public string Unit { get; set; }
        public decimal VATPercent { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        [NotMapped]
        public string Total
        {
            get
            {
                return (decimal.Parse(TotalVATless) + decimal.Parse(VATAmount)).ToString();
            }
        }
        [NotMapped]
        public string TotalVATless
        {
            get
            {
                return (UnitPrice * Quantity).ToString();
            }
        }
        [NotMapped]
        public string VATAmount
        {
            get
            {
                decimal VATless = UnitPrice * Quantity;
                decimal percent = VATless / 100.0m;

                return (percent * VATPercent).ToString();
            }
        }
        #endregion

        public Product()
        {
            Name = Unit = string.Empty;
            UnitPrice = 0L;
            Quantity = 0.0m;

            Bills = new List<Bill>();
        }
    }
}
