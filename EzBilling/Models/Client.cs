using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzBilling.Models
{
    public class Client
    {
        #region Properties
        public long ClientId { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public virtual IList<Bill> Bills { get; set; }

        // For being lazy and so that BillWriter can understand this.
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

        public Client()
        {
            Name = string.Empty;

            Bills = new List<Bill>();
            Address = new Address();
        }
    }
}
