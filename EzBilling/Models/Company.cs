using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzBilling.Models
{
    public class Company
    {
        #region Properties
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
        
        // For being lazy and so that the BillWriter can understand this.
        [NotMapped]
        public string Street
        {
            get
            {
                return Address.Street;
            }
            set
            {
                Address.Street = value;
            }
        }
        [NotMapped]
        public string City
        {
            get
            {
                return Address.City;
            }
            set
            {
                Address.City = value;
            }
        }
        [NotMapped]
        public string PostalCode
        {
            get
            {
                return Address.PostalCode;
            }
            set
            {
                Address.PostalCode = value;
            }
        }
        #endregion

        public Company()
        {
            CompanyID = Name = BankName = BankBIC = AccountNumber = BillerName = Email = Phone = string.Empty;

            Bills = new List<Bill>();
            Address = new Address();
        }
    }
}
