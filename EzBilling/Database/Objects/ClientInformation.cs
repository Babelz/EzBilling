using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using EzBilling.Database.Serialization;

namespace EzBilling.Database.Objects
{
    [DatabaseObjectAttribute("Clients", "Client")]
    public sealed class ClientInformation 
    {
        #region Properties
        [DatabaseObjectFieldAttribute]
        public string Name
        {
            get;
            set;
        }
        [DatabaseObjectFieldAttribute]
        public string Street
        {
            get;
            set;
        }
        [DatabaseObjectFieldAttribute]
        public string City
        {
            get;
            set;
        }
        [DatabaseObjectFieldAttribute]
        public string PostalCode
        {
            get;
            set;
        }
        #endregion

        public ClientInformation()
        {
            Name = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            PostalCode = string.Empty;
        }
    }
}
