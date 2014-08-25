using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace EzBilling.DatabaseObjects
{
    public sealed class EzBillingDatabase
    {
        #region Vars
        private readonly DatabaseHelper database;
        private readonly string databaseFile;
        #endregion

        #region Ctor
        public EzBillingDatabase()
        {
            databaseFile = "db";
            database = new DatabaseHelper(databaseFile);
            Initialize();
        }
        #endregion

        public void Initialize()
        {
            if (File.Exists(databaseFile)) return;

            SQLiteConnection.CreateFile(databaseFile);
            database.ExecuteFile("sql\\ezbilling.sql");
        }


        #region Methods

        #region Client

        public ClientInformation GetClientInformation(string id)
        {
            database.Open();
            DataTable result = database.Select("SELECT * FROM client where client_id = @client_id", new Dictionary<string, object>
            {
                {"@client_id", id}
            });
            if (result.Rows.Count == 0) throw new ArgumentException("There is no client_id " + id, "id");
            ClientInformation c = new ClientInformation();
            c.Fill(result.Rows[0]);
            database.Close();
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

        public bool Update(ClientInformation c)
        {
            int rows = database.Update("client", new Dictionary<string, object>
            {
                { "name", c.Name },
                { "city", c.City },
                { "postal_code", c.PostalCode },
                { "address", c.Street }
            }, "client_id", c.ID);
            return rows > 0;
        }

        public bool Delete(ClientInformation c)
        {
            
            return database.Delete("client", new Dictionary<string, string>
            {
                {"client_id", c.ID}
            }) > 0;
        }

        public void Insert(ClientInformation c)
        {
            database.Insert("client", new Dictionary<string, object>
            {
                { "client_id", null },
                { "name", c.Name },
                { "city", c.City },
                { "postal_code", c.PostalCode },
                { "address", c.Street }
            });
            c.ID = "" + database.LastInsertRowId();
        }

        public List<ClientInformation> GetClientsByName(string name)
        {
            List<ClientInformation> list = new List<ClientInformation>();
            var result = database.Select("SELECT * FROM Client WHERE name = @name", new Dictionary<string, object>()
            {
                {"@name", name}
            });
            foreach (DataRow row in result.Rows)
            {
                ClientInformation c = new ClientInformation();
                c.Fill(row);
                list.Add(c);
            }
            return list;
        }

        #endregion

        #region Company
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

        #region Product

        //public 

        #endregion

        #region Bills

        public List<BillInformation> GetAssociatedBills(CompanyInformation company, ClientInformation client)
        {
            List<BillInformation> bills = new List<BillInformation>();
            DataTable results = database.Select("SELECT * FROM bill WHERE company=@company AND client=@client",
                new Dictionary<string, object>()
                {
                    {"company", company.ID},
                    {"client", client.ID}
                });

            foreach (DataRow result in results.Rows)
            {
                BillInformation bill = new BillInformation();
                bill.Fill(result);
                bills.Add(bill);
            }
            return bills;
        }

        public List<ProductInformation> GetProducts(BillInformation bill)
        {
            List<ProductInformation> products = new List<ProductInformation>();
            DataTable results = database.Select("SELECT " +
                            "p.product_id, p.name, p.quantity, p.unit_price, p.unit, p.vat FROM product AS p " +
                            "INNER JOIN BillItems AS i ON p.product_id = i.product_id " +
                            "WHERE i.bill_id = @bill", new Dictionary<string, object>()
                            {
                                {"bill", bill.ID}
                            });
            foreach (DataRow dataRow in results.Rows)
            {
                ProductInformation product = new ProductInformation();
                product.Fill(dataRow);
                
                products.Add(product);
            }
            return products;
        }

        #endregion


        #endregion
    }
}
