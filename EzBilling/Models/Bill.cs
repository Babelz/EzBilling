using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzBilling.Models
{
    public partial class Bill
    {
        #region Properties
        public long BillID { get; set; }
        public string CompanyID { get; set; }
        public long ClientID { get; set; }
        public string Reference { get; set; }
        public string DueDate { get; set; }
        public string AdditionalInformation { get; set; }
        public virtual Company Company { get; set; }
        public virtual Client Client { get; set; }
        public virtual IList<Product> Products { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string Total
        {
            get
            {
                decimal total = 0.0m;

                for (int i = 0; i < Products.Count; i++)
                {
                    total += decimal.Parse(Products[i].Total);
                }

                return total.ToString("00.00");
            }
        }
        [NotMapped]
        public string TotalVATless
        {
            get
            {
                decimal total = 0.0m;

                for (int i = 0; i < Products.Count; i++)
                {
                    total += decimal.Parse(Products[i].TotalVATless);
                }

                return total.ToString("00.00");
            }
        }
        [NotMapped]
        public string VATAmount
        {
            get
            {
                decimal total = 0.0m;

                for (int i = 0; i < Products.Count; i++)
                {
                    total += decimal.Parse(Products[i].VATAmount);
                }

                return total.ToString("00.00");
            }
        }
        #endregion

        public Bill()
        {
            DueDate = Reference = AdditionalInformation = Name = string.Empty;
            

            this.Products = new List<Product>();
        }
    }
}
