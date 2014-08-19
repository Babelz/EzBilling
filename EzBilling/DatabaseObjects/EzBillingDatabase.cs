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

        #region Client

        public ClientInformation GetClientInformation(string id)
        {
            DataTable result = database.Select("SELECT * FROM client where client_id = @client_id", new Dictionary<string, object>
            {
                {"@client_id", id}
            });
            if (result.Rows.Count == 0) throw new ArgumentException("There is no client_id " + id, "id");
            ClientInformation c = new ClientInformation();
            c.Fill(result.Rows[0]);
            return c;
        }

        public List<ClientInformation> GetClientInformations()
        {
            database.Open();
            List<ClientInformation> r = new List<ClientInformation>();
            DataTable table = database.Select("select * from client");
            foreach (DataRow row in table.Rows)
            {
                ClientInformation c = new ClientInformation();
                c.Fill(row);
                r.Add(c);
            }

            database.Close();
            return r;
        }

        #endregion

        public List<CompanyInformation> GetCompanyInformations()
        {
            List<CompanyInformation> companies = new List<CompanyInformation>();
            database.Open();

            DataTable table = database.Select("select * from company");

            foreach (DataRow row in table.Rows)
            {
                CompanyInformation c = new CompanyInformation();
                c.Fill(row);

                companies.Add(c);
            }

            database.Close();
            return companies;
        }

        


        #endregion
    }
}
