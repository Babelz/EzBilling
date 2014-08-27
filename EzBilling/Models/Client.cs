using System;
using System.Collections.Generic;

namespace EzBilling.Models
{
    public class Client
    {
        public Client()
        {
            Bills = new List<Bill>();
            Address = new Address();
        }

        public long ClientId { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
