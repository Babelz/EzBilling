﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBilling.Models
{
    [ComplexType]
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public Address()
        {
            City = Street = PostalCode = string.Empty;
        }
    }
}
