using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EzBilling.DatabaseObjects
{
    public sealed class EzBillingDatabase
    {
        #region Vars
        private readonly DatabaseHelper database;
        #endregion

        #region Ctor
        public EzBillingDatabase()
        {
            database = new DatabaseHelper("db");
        }
        #endregion


        #region Methods

        public List<ClientInformation> GetClientInformations()
        {
            database.Open();
            List<ClientInformation> r = new List<ClientInformation>();
            DataTable table = database.Select("select * from client");
            foreach (DataRow row in table.Rows)
            {
                ClientInformation c = new ClientInformation();
                c.ID = row["client_id"].ToString();
                c.Name = row["name"].ToString();
                c.PostalCode = row["postal_code"].ToString();
                c.Street = row["address"].ToString();
                c.City = row["city"].ToString();
                r.Add(c);
            }

            database.Close();
            return r;
        }


        #endregion
    }
}
