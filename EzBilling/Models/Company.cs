using System;
using System.Collections.Generic;

namespace EzBilling.Models
{
    public class Company
    {
        public Company()
        {
            Bills = new List<Bill>();
            Address = new Address();
        }

        public string CompanyID { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string BankName { get; set; }
        public string BankBIC { get; set; }
        public string AccountNumber { get; set; }
        public string BillerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
