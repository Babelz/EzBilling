using System;
using System.Collections.Generic;
using System.Data;

namespace EzBilling.Database
{
    public sealed class BillInformation : DatabaseObject
    {
        #region Vars
        private string company;
        private string client;
        private string name;
        private string reference;
        private string dueDate;
        private string additionalInformation;
        private List<ProductInformation> products;
        #endregion

        #region Properties
        public string Company
        {
            get
            {
                return company;
            }
            set
            {
                company = value;
            }
        }
        public string Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
            }
        }
        public string DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
            }
        }
        public string AdditionalInformation
        {
            get
            {
                return additionalInformation;
            }
            set
            {
                additionalInformation = value;
            }
        }
        public List<ProductInformation> Products
        {
            get
            {
                return products;
            }
            set
            {
                products = value;
            }
        }
        public string VATAmount
        {
            get
            {
                decimal totalVATAmount = 0.0m;

                for (int i = 0; i < products.Count; i++)
                {
                    totalVATAmount += decimal.Parse(products[i].VATAmount);
                }

                return totalVATAmount.ToString("0.00");
            }
        }
        public string Total
        {
            get
            {
                decimal total = 0.0m;

                for (int i = 0; i < products.Count; i++)
                {
                    total += decimal.Parse(products[i].Total);
                }

                return total.ToString("0.00");
            }
        }
        public string TotalVATless
        {
            get
            {
                return (decimal.Parse(Total) - decimal.Parse(VATAmount)).ToString("0.00");
            }
        }
        #endregion

        public BillInformation()
        {
            products = new List<ProductInformation>();
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(name) ||
                   string.IsNullOrEmpty(reference) ||
                   string.IsNullOrEmpty(dueDate) ||
                   string.IsNullOrEmpty(additionalInformation) ||
                   products.Count > 0;
        }
        public override void Fill(DataRow info)
        {
            ID = info["bill_id"].ToString();
            Company = info["company"].ToString();
            Client = info["client"].ToString();
            Reference = info["reference"].ToString();
            DueDate = info["due_date"].ToString();
            AdditionalInformation = info["comments"].ToString();

        }
    }
}
