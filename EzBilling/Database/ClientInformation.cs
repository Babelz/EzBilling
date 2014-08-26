using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EzBilling.Database
{
    public sealed class ClientInformation : UserInformation
    {
        public ClientInformation()
            : base()
        {
        }

        public override void Fill(DataRow row)
        {
            ID = row["client_id"].ToString();
            Name = row["name"].ToString();
            PostalCode = row["postal_code"].ToString();
            Street = row["address"].ToString();
            City = row["city"].ToString();
        }
    }
}
