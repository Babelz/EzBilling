using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzBilling.Models
{
    public partial class Bill
    {
        public Bill()
        {
            this.Products = new List<Product>();
        }

        public long BillID { get; set; }
        public string CompanyID { get; set; }
        public long ClientID { get; set; }
        public string Reference { get; set; }
        public long DueDate { get; set; }
        public string AdditionalInformation { get; set; }
        public virtual Company Company { get; set; }
        public virtual Client Client { get; set; }
        public virtual IList<Product> Products { get; set; }
        [NotMapped]
        public string Name { get; set; }
    }
}
